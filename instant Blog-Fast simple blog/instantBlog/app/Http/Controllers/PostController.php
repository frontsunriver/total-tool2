<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Validation\Rule;
use App\Models\Post;

class PostController extends Controller
{
    public function __construct()
    {
        $this->middleware('can:moderator-post');
    }

    public function index()
    {
        //Get latest and where live posts and paginate them
        $posts = Post::orderBy('id', 'DESC')
            ->wherePostLive(1)
            ->simplePaginate(30);

        return view('posts.index', compact('posts'));
    }

    public function unpublished()
    {
        //Get latest and where unpublished posts and paginate them
        $posts = Post::orderBy('id', 'DESC')
            ->wherePostLive(0)
            ->simplePaginate(30);

        return view('posts.unpublished', compact('posts'));
    }


    public function multiple(Request $request)
    {
        if ($request->mulbtn === 'Delete') {
            if ($request->multiple > 0) {
                $posts = Post::findOrFail($request->multiple);
                foreach ($posts as $post) {
                    $post->tags()->detach();
                }
                Post::destroy($request->multiple);

                session()->flash('message', 'Posts Deleted!');
                return redirect('/contents');
            } else {
                session()->flash('error', 'Select Checkboxes!');
                return redirect('/contents');
            }
        } elseif ($request->mulbtn === 'Unpublish') {
            if ($request->multiple > 0) {
                Post::whereIn('id', $request->multiple)->update(['post_live' => 0]);

                session()->flash('message', 'Posts Unpublished!');
                return redirect('/contents');
            } else {
                session()->flash('error', 'Select Checkboxes!');
                return redirect('/contents');
            }
        } elseif ($request->mulbtn === 'Publish') {
            if ($request->multiple > 0) {
                Post::whereIn('id', $request->multiple)->update(['post_live' => 1]);

                session()->flash('message', 'Posts Published!');
                return redirect('/contents');
            } else {
                session()->flash('error', 'Select Checkboxes!');
                return redirect('/contents');
            }
        }
    }
}
