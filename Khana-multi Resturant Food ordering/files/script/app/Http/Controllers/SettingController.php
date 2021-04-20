<?php

namespace App\Http\Controllers;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Post;
use App\Page;
use App\Category;
use samdark\sitemap\Sitemap;
use samdark\sitemap\Index;
class SettingController extends Controller
{
	public function sitemapstore()
	{
		
		$index = new Index(base_path('sitemap.xml'));
		 foreach (Post::get() as  $post) {
            $url=url('/').'/post/'.$post->slug;
            $index->addSitemap($url,time(), Sitemap::DAILY, 0.3);       
        }
        foreach (Page::get() as  $post) {
            $url=url('/').'/page/'.$post->slug;
            $index->addSitemap($url,time(), Sitemap::DAILY, 0.3);       
        }
        foreach (Category::get() as  $page) {
            $url=url('/').'/category/'.$page->slug;
            $index->addSitemap($url,time(), Sitemap::DAILY, 0.3);       
        }
		$index->write();		
	}
	public function sitemapView()
	{
		
		return response(file_get_contents(base_path('sitemap.xml')), 200, [
    		'Content-Type' => 'application/xml'
		]);
	}
}
