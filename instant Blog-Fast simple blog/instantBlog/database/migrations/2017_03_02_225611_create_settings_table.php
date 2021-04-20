<?php

use Illuminate\Support\Facades\Schema;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Database\Migrations\Migration;

class CreateSettingsTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('settings', function (Blueprint $table) {
            $table->id();
            $table->string('site_name', 191)->nullable();
            $table->text('site_desc')->nullable();
            $table->string('site_title', 191)->nullable();
            $table->boolean('allow_comments')->default(0);
            $table->boolean('allow_users')->default(0);
            $table->boolean('check_cont')->default(0);
            $table->string('site_logo', 191)->default('logo.png');
            $table->string('site_extra', 191)->nullable();
            $table->string('home_link', 41)->nullable();
            $table->string('pop_link', 41)->nullable();
            $table->string('cat_link', 41)->nullable();
            $table->string('arch_link', 41)->nullable();
            $table->string('search_link', 41)->nullable();
            $table->string('login_link', 41)->nullable();
            $table->text('post_ads')->nullable();
            $table->text('page_ads')->nullable();
            $table->text('between_ads')->nullable();
            $table->boolean('fb_publishing')->default(0);
            $table->string('fb_theme', 191)->default('default');
            $table->text('fb_ads_code')->nullable();
            $table->text('fb_page_token')->nullable();
            $table->boolean('amp_ad_server')->default(0);
            $table->text('amp_adscode')->nullable();
            $table->text('footer')->nullable();
            $table->text('site_analytic')->nullable();
            $table->boolean('site_instant')->default(0);
            $table->string('site_activation', 15)->nullable();
        });
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        Schema::dropIfExists('settings');
    }
}
