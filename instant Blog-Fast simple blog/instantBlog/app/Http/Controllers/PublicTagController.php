<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Validation\Rule;
use App\Models\Tag;
use App\Models\Page;

class PublicTagController extends Controller
{
    public function index(Tag $tag)
    {
        $posts = $tag
        ->posts()
        ->wherePostLive(1)
        ->paginate(30);

        $pages = Page::orderBy('id', 'ASC') ->get();

        return view('public.index', compact('posts', 'pages', 'tag'));
    }

    public function tags()
    {
        return view('public.tags');
    }
}
