<?php

namespace App\Http\Controllers;

use App\Models\Page;
use App\Models\Post;
use App\Models\User;
use Illuminate\Http\Request;
use Validator;

class PublicPostController extends Controller
{
    public function index()
    {
        //Get latest and where live posts and paginate them
        $posts = Post::latest()
            ->wherePostLive(1)
            ->paginate(30);

        $pages = Page::orderBy('id', 'ASC')->get();

        return view('public.index', compact('posts', 'pages'));
    }

    //Show single post
    public function show(Post $post)
    {
        $nextid     = Post::where('id', '>', $post->id)->wherePostLive(1)->min('id');
        $previousid = Post::where('id', '<', $post->id)->wherePostLive(1)->max('id');

        if ($nextid) {
            $next = Post::find($nextid)->post_slug;
        } else {
            $next = null;
        }

        if ($previousid) {
            $previous = Post::find($previousid)->post_slug;
        } else {
            $previous = null;
        }

        $random = Post::inRandomOrder()
            ->wherePostLive(1)
            ->first()
            ->post_slug;

        $related = Post::inRandomOrder()
            ->wherePostLive(1)
            ->where('id', '!=', $post->id)
            ->take(5)
            ->get();

        $pages = Page::orderBy('id', 'ASC')->get();

        if ($post->edit_id) {
            $editby = User::where('id', $post->edit_id)->first();
        } else {
            $editby = null;
        }

        return view('public.show', compact('post', 'next', 'previous', 'random', 'related', 'pages', 'editby'));
    }
    //Show archives posts
    public function archives()
    {
        return view('public.archives');
    }
    //Show single archiveposts
    public function archiveposts(Request $request)
    {
        $month = request(['month']);

        $validator = Validator::make($month, [
            'month' => 'required|
            in:January,February,March,April,May,June,July,August,September,October,November,December',
        ]);

        if ($validator->fails()) {
            return redirect('/archives');
        }

        $posts = Post::latest()
            ->filter(request(['month', 'year']))
            ->wherePostLive(1)
            ->paginate(30);

        return view('public.archiveposts', compact('posts'));
    }
    //Find Popular contents
    public function popular()
    {
        $posts = Post::wherePostLive(1)
            ->withCount('likes')
            ->orderBy('likes_count', 'desc')
            ->paginate(30);

        return view('public.popular', compact('posts'));
    }

    //Search content
    public function search(Request $request)
    {
        $s = $request->input('s');

        $posts = Post::latest()
            ->search($s)
            ->wherePostLive(1)
            ->paginate(30);

        $pages = Page::orderBy('id', 'ASC')->get();

        return view('public.index', compact('posts', 'pages'));
    }

    //Show single post for facebook
    public function facebookShow()
    {
        $posts = Post::latest()
            ->wherePostLive(1)
            ->take(10)
            ->get();

        $related = Post::inRandomOrder()
            ->wherePostLive(1)
            ->where('id', '!=', $post->id)
            ->take(4)
            ->get();

        return view('public.facerss', compact('posts', 'related'));
    }

    //Show single post for facebook
    public function ampShow(Post $post)
    {
        $related = Post::inRandomOrder()
            ->wherePostLive(1)
            ->where('id', '!=', $post->id)
            ->take(5)
            ->get();
            
        return view('public.showamp', compact('post', 'related'));
    }

    public function showPage(Page $page)
    {
        $pages = Page::orderBy('id', 'ASC')->get();
        return view('public.showpage', compact('page', 'pages'));
    }

    public function feedControl()
    {
        $posts = Post::latest()
            ->wherePostLive(1)
            ->paginate(30);

        return response()
            ->view('public.feed', compact('posts'))
            ->header('Content-Type', 'application/xml');
    }
}
