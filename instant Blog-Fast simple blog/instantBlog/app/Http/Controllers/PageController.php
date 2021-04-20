<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Validation\Rule;
use App\Models\Page;
use Image;

class PageController extends Controller
{

    public function __construct()
    {
        $this->middleware('can:admin-area');
    }

    public function index()
    {
        $pages = Page::orderBy('id', 'DESC')
            ->paginate(30);

        return view('posts.pages', compact('pages'));
    }

    public function create()
    {
        return view('pages.create');
    }

    public function store(Request $request)
    {
        $attributes = request(['page_title', 'page_slug', 'page_content']);

        $this->validate(request(), [
            'page_title' => 'required|unique:pages,page_title',
            'page_slug' => 'required|unique:pages,page_slug',
        ]);

        if(!empty($request->page_content)){
            $message = $attributes['page_content'];
            $dom = new \DomDocument();
            $dom->loadHtml( mb_convert_encoding($message, 'HTML-ENTITIES', "UTF-8"), LIBXML_HTML_NOIMPLIED | LIBXML_HTML_NODEFDTD);
            $images = $dom->getElementsByTagName('img');
            foreach($images as $img){
                $src = $img->getAttribute('src');
                if(preg_match('/data:image/', $src)){                
                    preg_match('/data:image\/(?<mime>.*?)\;/', $src, $groups);
                    $mimetype = $groups['mime'];                
                    $filename = uniqid();
                    $filepath = "/uploads/$filename.$mimetype";
                    $image = Image::make($src)
                      ->encode($mimetype, 100)
                      ->save(public_path($filepath));                
                    $new_src = asset($filepath);
                    $img->removeAttribute('src');
                    $img->setAttribute('src', $new_src);
                }
            }
            $attributes['page_content'] = $dom->saveHTML();
        }

        Page::create($attributes);

        session()->flash('message', 'Page Created!');
        return redirect('/pages');
    }

    public function show($id)
    {
        $page = Page::findOrFail($id);
        return view('pages.show', compact('page'));
    }

    public function edit($id)
    {
        $page = Page::findOrFail($id);
        return view('pages.edit', compact('page'));
    }

    public function update(Request $request, $id)
    {
        $page = Page::findOrFail($id);

        $attributes = request(['page_title', 'page_slug', 'page_content']);

        $this->validate(request(), [
            'page_title' => [
                'required',
                Rule::unique('pages')->ignore($page->id),
            ],

            'page_slug' => [
                'required',
                Rule::unique('pages')->ignore($page->id),
            ],

        ]);

        if(!empty($request->page_content)){
            $message = $attributes['page_content'];
            $dom = new \DomDocument();
            $dom->loadHtml( mb_convert_encoding($message, 'HTML-ENTITIES', "UTF-8"), LIBXML_HTML_NOIMPLIED | LIBXML_HTML_NODEFDTD);   
            $images = $dom->getElementsByTagName('img');
            foreach($images as $img){
                $src = $img->getAttribute('src');
                if(preg_match('/data:image/', $src)){                
                    preg_match('/data:image\/(?<mime>.*?)\;/', $src, $groups);
                    $mimetype = $groups['mime'];                
                    $filename = uniqid();
                    $filepath = "/uploads/$filename.$mimetype"; 
                    $image = Image::make($src)
                      ->encode($mimetype, 100)
                      ->save(public_path($filepath));                
                    $new_src = asset($filepath);
                    $img->removeAttribute('src');
                    $img->setAttribute('src', $new_src);
                }
            }
            $attributes['page_content'] = $dom->saveHTML();
        }
        
        $page->update($attributes);

        session()->flash('message', 'Page Updated!');
        return redirect('/pages');
    }

    public function destroy($id)
    {
        $page = Page::findOrFail($id);
        $page->delete();
        session()->flash('message', 'Page Deleted!');
        return redirect('/pages');
    }
}
