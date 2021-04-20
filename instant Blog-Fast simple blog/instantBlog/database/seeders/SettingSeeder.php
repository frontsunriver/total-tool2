<?php

namespace Database\Seeders;

use Illuminate\Database\Seeder;
use App\Models\Setting;

class SettingSeeder extends Seeder
{
    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {    	
        Setting::firstOrCreate([
            'id' => '1'],
            ['site_name' => 'Your Site Name',
            'site_desc' => 'Your Description',
            'site_title' => 'Site Title',
            'home_link' => 'Home',
            'pop_link' => 'Popular',
            'cat_link' => 'Categories',
            'arch_link' => 'Archives',
            'search_link' => 'Search',
            'login_link' => 'Login'
        ]);
    }
}
