<?php

use Illuminate\Database\Seeder;
use App\Role;

class RolesTableSeeder extends Seeder
{
    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {
        Role::create([
        	'name' => 'Admin',
        	'slug' => 'admin',
        ]);

        Role::create([
        	'name' => 'Author',
        	'slug' => 'author',
        ]);

        Role::create([
            'name' => 'Store',
            'slug' => 'store',
        ]);

        Role::create([
            'name' => 'Rider',
            'slug' => 'rider',
        ]);
    }
}
