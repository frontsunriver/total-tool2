<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Validation\Rule;
use App\Models\Post;
use App\Models\Content;
use App\Models\Setting;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Gate;
use Image;
use Validator;

class HomeController extends Controller
{
    public function __construct()
    {
        $this->middleware('auth');
        $this->middleware('throttle:10,10', ['only' => ['store']]);
    }

    public function index()
    {
        if (Gate::allows('moderator-post')) {
            $posts = Post::orderBy('id', 'DESC')
                ->wherePostLive(0)
                ->simplePaginate(15);
        } else {
            $posts = Post::orderBy('id', 'DESC')
                ->whereUserId(auth()->user()->id)
                ->wherePostLive(0)
                ->simplePaginate(15);
        }

        return view('home', compact('posts'));
    }

    public function store(Request $request)
    {
        $attributes = request(['post_title', 'post_instant', 'post_color', 'post_slug', 'post_desc', 'post_media', 'post_video']);

        $validator = Validator::make($request->all(), [
            'content.*' => 'required',
            'post_title' => 'required|unique:posts,post_title|min:5',
        ]);
        $attributes['post_slug'] = null;
        $attributes['user_id'] = Auth::user()->id;

        if(auth()->user()->can('publish-post')) {
            $attributes['post_live'] = '1';
        } else {
            $attributes['post_live'] = Setting::first()->check_cont;
        }

        if (!empty($request->post_video)) {
            $attributes['post_video'] = substr($request->post_video, strrpos($request->post_video, 'v=') + 2);
            $attributes['video_url'] = $request->post_video;
        }

        if ($validator->passes()) {
            if ($request->has('content')) {
                $types = $request->input('type');
                $contents = $request->input('content');
                $social = array('tweet', 'facebook', 'instagram', 'pinterest');

                foreach ($contents as $key => $value) {
                    $collections[$key] = [
                        'type' => $types[$key],
                        'body' => $value
                    ];

                    if ($types[$key] == 'youtube') {
                        $collections[$key]['body'] = substr($value, strrpos($value, 'v=') + 2);
                    }

                    if (in_array($types[$key], $social)) {
                        $collections[$key]['embed_id'] = $value;
                    } else {
                        $collections[$key]['embed_id'] = null;
                    }
                }
            }

            $post = Post::create($attributes);
            $postid = $post->id;
            
            if (isset($request->tag_id)) {
                $post->tags()->sync(request(['tag_id']));
            } else {
                $post->tags()->sync(array());
            }
            
            if ($request->has('content')) {
                foreach ($collections as $collection) {
                    $collection['post_id'] = $postid;
                    Content::create($collection);
                }
            }

            return response()->json(['success'=>__('messages.postcreated')]);
        }

        return response()->json(['error'=>$validator->errors()->all()]);
    }

    public function show($id)
    {
        //Show single post
        $post = Post::findOrFail($id);
        return view('member.show', compact('post'));
    }

    public function edit($id)
    {
        //Edit single post
        $post = Post::findOrFail($id);
        $postTag = Post::findOrFail($id)->tags()->withPivot('tag_id')->first();
        return view('member.edit', compact('post', 'postTag'));
    }

    public function update(Request $request, $id)
    {
        //Update edited single post
        $post = Post::findOrFail($id);

        $attributes = request(['post_title', 'post_slug', 'post_instant', 'post_color', 'post_desc', 'post_media', 'post_video', 'post_live', 'edit_id']);

        $attributes['edit_id'] = Auth::user()->id;
        
        if (!empty($request->post_video)) {
            $attributes['post_video'] = substr($request->post_video, strrpos($request->post_video, 'v=') + 2);
            $attributes['video_url'] = $request->post_video;
        }

        if (empty($request->post_color)) {
            if (!empty($post->post_color)) {
                $attributes['post_color'] = $post->post_color;
            } else {
                $attributes['post_color'] = 'bg-dark';
            }
        }

        $validator = Validator::make($request->all(), [
            'post_title' => [
                'required',
                Rule::unique('posts')->ignore($post->id),
                'min:5',
            ],

            'post_slug' => [
                'required',
                Rule::unique('posts')->ignore($post->id),
            ],

            'content.*' => 'required'
        ]);

        if ($validator->passes()) {
            if ($request->has('content')) {
                $types = $request->input('type');
                $ids = $request->input('id');
                $contents = $request->input('content');
                $social = array('tweet', 'facebook', 'instagram', 'pinterest');

                foreach ($contents as $key => $value) {
                    $collections[$key] = [
                        'id' => isset($ids[$key]) ? $ids[$key] : null,
                        'post_id' => $id,
                        'type' => $types[$key],
                        'body' => $value
                    ];

                    if ($types[$key] == 'youtube') {
                        $collections[$key]['body'] = substr($value, strrpos($value, 'v=') + 2);
                    }

                    if (in_array($types[$key], $social)) {
                        $collections[$key]['embed_id'] = $value;
                    } else {
                        $collections[$key]['embed_id'] = null;
                    }
                }
            }

            if ($request->has('post_instant')) {
                $attributes['post_instant'] = '1';
            } else {
                $attributes['post_instant'] = '0';
            }


            if ($request->has('post_live')) {
                $post->update($attributes);
            } else {
                $attributes['post_live'] = '0';
                $post->update($attributes);
            }

            if (isset($request->tag_id)) {
                $post->tags()->sync(request(['tag_id']));
            } else {
                $post->tags()->sync(array());
            }

            if ($request->has('content')) {
                foreach ($collections as $collection) {
                    if (!empty($collection['id'])) {
                        Content::whereId($collection['id'])->update($collection);
                    } else {
                        Content::create($collection);
                    }
                }
            }

            return response()->json(['success'=>__('messages.postedited')]);
        }

        return response()->json(['error'=>$validator->errors()->all()]);
    }

    public function destroy($id)
    {
        //Delete single post
        $post = Post::findOrFail($id);
        $post->tags()->detach();


        if ($post->contents) {
            foreach ($post->contents as $item) {
                if ($item->embed) {
                    $item->embed->delete();
                }
            }
        }

        $post->contents()->delete();
        $post->delete();

        session()->flash('message', __('messages.postdeleted'));
        return redirect('/home');
    }

    public function addpost()
    {
        return view('member.add');
    }

    public function delcontent(Request $request)
    {
        //Delete single content
        $delid = $request->id;
        $content = Content::findOrFail($delid);
        if(!empty($content->embed)) {
            $content->embed->delete();
        }        
        $success = $content->delete();

        if ($success) {
            return response()->json(['success'=> __('messages.contdeleted')]);
        }
            
        return response()->json(['error'=> __('messages.contnotdeleted')]);
    }
}
