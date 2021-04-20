<?php

use Illuminate\Database\Seeder;
use App\Options;
class OptionTableSeeder extends Seeder
{
    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {
       Options::create([
        'key'=>'system_basic_info',
        'value'=>'{"number":"018000","logo":null}',
       ]);
       Options::create([
        'key'=>'seo',
        'value'=>'{"title":"LPress","description":null,"canonical":null,"tags":null,"twitterTitle":null}',
       ]);
       Options::create([
        'key'=>'langlist',
        'value'=>'{"English":"en","Bengali":"bn"}',
       ]);
       
       Options::create([
        'key'=>'lp_imagesize',
        'value'=>'[{"key":"small","height":"80","width":"80"},{"key":"medium","height":"186","width":"255"}]',
       ]);
       Options::create([
        'key'=>'lp_filesystem',
        'value'=>'{"compress":5,"system_type":"local","system_url":null}',
       ]);
       Options::create([
        'key'=>'lp_perfomances',
        'value'=>'{"lazyload":0,"image":"uploads\/lazy.png"}',
       ]);

       Options::create([
        'key'=>'rider_commission',
        'value'=>'10',
       ]);
       Options::create([
        'key'=>'currency_name',
        'value'=>'USD',
       ]);
       Options::create([
        'key'=>'currency_icon',
        'value'=>'$',
       ]);
       Options::create([
        'key'=>'km_rate',
        'value'=>'100',
       ]);

       Options::create([
        'key'=>'default_map',
        'value'=>'{"default_lat":"23.685","default_long":"90.3563","default_zoom":"10"}',
       ]);

     
    }
}
