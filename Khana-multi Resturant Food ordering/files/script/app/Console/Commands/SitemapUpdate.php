<?php

namespace App\Console\Commands;

use Illuminate\Console\Command;
use App\Post;
use App\Page;
use App\Category;
use samdark\sitemap\Sitemap;
use samdark\sitemap\Index;
class SitemapUpdate extends Command
{
    /**
     * The name and signature of the console command.
     *
     * @var string
     */
    protected $signature = 'daily:update-sitemap';

    /**
     * The console command description.
     *
     * @var string
     */
    protected $description = 'sitemap will genarate every 1 day';

    /**
     * Create a new command instance.
     *
     * @return void
     */
    public function __construct()
    {
        parent::__construct();
    }

    /**
     * Execute the console command.
     *
     * @return mixed
     */
    public function handle()
    {
        $index = new Index(base_path('sitemap.xml'));
        $url=url('/');
        foreach (Post::get() as  $post) {
            $url=url('/').'/post/'.$post->slug;
            $index->addSitemap($url,time(), Sitemap::DAILY, 0.3);       
        }
        
        foreach (Page::get() as  $page) {
            $url=url('/').'/page/'.$page->slug;
            $index->addSitemap($url,time(), Sitemap::DAILY, 0.3);       
        }
         $url=url('/').'/prevention';
        $url=url('/').'/faq';
        $url=url('/').'/blog';
        $url=url('/').'/post';
        $url=url('/').'/about';
        $url=url('/').'/contact-us';
        $index->write();    
    }
}
