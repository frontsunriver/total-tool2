<?php

namespace App\Http\Controllers;

use App\Models\Tag;
use Illuminate\Http\Request;
use Illuminate\Validation\Rule;
use Image;

class TagController extends Controller
{
    public function __construct()
    {
        $this->middleware('can:admin-area');
    }
    
    public function index()
    {
        $tags = Tag::orderBy('id', 'DESC')
            ->paginate(30);

        return view('posts.tags', compact('tags'));
    }

    public function create()
    {
        return view('tags.create');
    }

    public function store(Request $request)
    {
        $attributes = request(['title', 'name', 'tag_media', 'color', 'desc']);

        $this->validate(request(), [
            'title' => 'required|unique:tags,title',
            'name' => 'required|unique:tags,name|max:25',
            'tag_media' => 'required|mimes:jpeg,png,jpg,gif,svg|max:2048',
        ]);

        if ($request->hasFile('tag_media')) {
            $postimage = $request->file('tag_media');
            $filename = time() . '.' . $postimage->getClientOriginalExtension();
            Image::make($postimage)->resize(400, 200)->save(public_path('/uploads/'. $filename));
            $attributes['tag_media'] = $filename;
        }

        Tag::create($attributes);

        session()->flash('message', 'Category Created!');
        return redirect('/cats');
    }

    public function show($id)
    {
        $tag = Tag::findOrFail($id);
        return view('tags.show', compact('tag'));
    }

    public function edit($id)
    {
        $tag = Tag::findOrFail($id);
        return view('tags.edit', compact('tag'));
    }

    public function update(Request $request, $id)
    {
        $tag = Tag::findOrFail($id);

        $attributes = request(['title', 'name', 'tag_media', 'color', 'desc']);

        $this->validate(request(), [
            'title' => [
                'required',
                Rule::unique('tags')->ignore($tag->id),
            ],

            'name' => [
                'required',
                'max:25',
                Rule::unique('tags')->ignore($tag->id),
            ],

            'tag_media' => 'mimes:jpeg,png,jpg,gif,svg|max:2048',
        ]);

        if ($request->hasFile('tag_media')) {
            $postimage = $request->file('tag_media');
            $filename = time() . '.' . $postimage->getClientOriginalExtension();
            Image::make($postimage)->resize(400, 200)->save(public_path('/uploads/'. $filename));
            $attributes['tag_media'] = $filename;
        } else {
            $attributes['tag_media'] = $tag->tag_media ;
        }

        $tag->update($attributes);

        session()->flash('message', 'Category Updated!');
        return redirect('/cats');
    }

    public function destroy($id)
    {
        $tag = Tag::findOrFail($id);
        $tag->posts()->detach();
        $tag->delete();
        session()->flash('message', 'Category Deleted!');
        return redirect('/cats');
    }
}