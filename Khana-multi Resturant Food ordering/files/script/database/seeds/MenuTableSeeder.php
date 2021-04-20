<?php

use Illuminate\Database\Seeder;
use App\Menu;
class MenuTableSeeder extends Seeder
{
    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {	
    	$url=url('/');

        Menu::create([
        	"name"=>"Header",
        	"position"=>"Header",
        	"data"=> '[{"text":"Home","href":"/","icon":"","target":"_self","title":""},{"text":"City","href":"#","icon":"fas fa-angle-down","target":"_self","title":"","children":[{"text":"Dhaka","href":"/area/dhaka","icon":"empty","target":"_self","title":""},{"text":"Chittagong","href":"/area/chittagong","icon":"empty","target":"_self","title":""},{"text":"Feni","href":"/area/feni","icon":"empty","target":"_self","title":""},{"text":"Bogra","href":"/area/bogra","icon":"empty","target":"_self","title":""},{"text":"Barisal","href":"/area/barisal","icon":"empty","target":"_self","title":""},{"text":"Rajshahi","href":"/area/rajshahi","icon":"empty","target":"_self","title":""},{"text":"Khulna","href":"/area/khulna","icon":"empty","target":"_self","title":""},{"text":"Rangpur","href":"/area/rangpur","icon":"empty","target":"_self","title":""}]},{"text":"Customer Area","href":"#","icon":"fas fa-angle-down","target":"_self","title":"","children":[{"text":"Login","href":"/user/login","icon":"empty","target":"_self","title":""},{"text":"Register","href":"/user/register","icon":"empty","target":"_self","title":""},{"text":"Customer Panel","href":"/author/dashboard","icon":"empty","target":"_self","title":""}]},{"text":"Rider Area","href":"#","icon":"fas fa-angle-down","target":"_self","title":"","children":[{"text":"Login","href":"/rider/login","icon":"empty","target":"_self","title":""},{"text":"Register","href":"/rider/register","icon":"empty","target":"_self","title":""},{"text":"Rider Panel","href":"/rider/dashboard","icon":"empty","target":"_self","title":""}]},{"text":"Restaurant Area","href":"#","icon":"fas fa-angle-down","target":"_self","title":"","children":[{"text":"Login","href":"/login","icon":"empty","target":"_self","title":""},{"text":"Register","href":"/restaurant/register","icon":"empty","target":"_self","title":""},{"text":"Restaurant Panel","href":"/store/dashboard","icon":"empty","target":"_self","title":""}]}]',
        	"lang"=>"en",
        	"status"=>1,
        ]);


        Menu::create([
            "name"=>"Header",
            "position"=>"Header",
            "data"=> '[{"text":"হোম","href":"/","icon":"","target":"_self","title":""},{"text":"আপানার শহর","href":"#","icon":"fas fa-angle-down","target":"_self","title":"","children":[{"text":"ঢাকা","href":"/area/dhaka","icon":"empty","target":"_self","title":""},{"text":"চট্রগ্রাম","href":"/area/chittagong","icon":"empty","target":"_self","title":""},{"text":" ফেণী","href":"/area/feni","icon":"empty","target":"_self","title":""},{"text":"বগুড়া","href":"/area/bogra","icon":"empty","target":"_self","title":""},{"text":"বরিশাল","href":"/area/barisal","icon":"empty","target":"_self","title":""},{"text":"রাজশাহী","href":"/area/rajshahi","icon":"empty","target":"_self","title":""},{"text":"খুলনা","href":"/area/khulna","icon":"empty","target":"_self","title":""},{"text":"রংপুর","href":"/area/rangpur","icon":"empty","target":"_self","title":""}]},{"text":"ক্রেতা","icon":"fas fa-angle-down","href":"#","target":"_self","title":"","children":[{"text":"লগইন","icon":"empty","href":"/user/login","target":"_self","title":""},{"text":"রেজিস্টার","icon":"empty","href":"/user/register","target":"_self","title":""},{"text":"ড্যাশবোর্ড","icon":"empty","href":"/author/dashboard","target":"_self","title":""}]},{"text":"রাইডার","icon":"fas fa-angle-down","href":"#","target":"_self","title":"","children":[{"text":"লগইন","icon":"empty","href":"/rider/login","target":"_self","title":""},{"text":"রেজিস্টার","icon":"empty","href":"/rider/register","target":"_self","title":""},{"text":"ড্যাশবোর্ড","icon":"empty","href":"/rider/dashboard","target":"_self","title":""}]},{"text":"রেস্তোরাঁ","icon":"fas fa-angle-down","href":"#","target":"_self","title":"","children":[{"text":"লগইন","icon":"empty","href":"/login","target":"_self","title":""},{"text":"রেজিস্টার","icon":"empty","href":"/restaurant/register","target":"_self","title":""},{"text":"ড্যাশবোর্ড","icon":"empty","href":"/store/dashboard","target":"_self","title":""}]}]',
            "lang"=>"bn",
            "status"=>1,
        ]);
       
        
    }
}
