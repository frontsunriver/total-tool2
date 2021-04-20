<?php

use Illuminate\Database\Seeder;




class DatabaseSeeder extends Seeder
{
    /**
     * Seed the application's database.
     *
     * @return void
     */
    public function run()
    {
        $this->call(RolesTableSeeder::class);
        $this->call(UsersTableSeeder::class);
        $this->call(OptionTableSeeder::class);
        $this->call(CategoryTableSeeder::class);
        $this->call(CustomTableSeeder::class);
        $this->call(ServiceTableSeeder::class);
        $this->call(BlogTableSeeder::class);
        $this->call(CustomizerTableSeeder::class);
        $this->call(MenuTableSeeder::class);
    }
}
