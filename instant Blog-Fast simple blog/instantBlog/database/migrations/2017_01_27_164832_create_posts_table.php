<?php

use Illuminate\Support\Facades\Schema;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Database\Migrations\Migration;

class CreatePostsTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('posts', function (Blueprint $table) {
            $table->id();
            $table->integer('user_id')->nullable();
            $table->boolean('post_live')->default(1);
            $table->string('post_instant', 51)->nullable();
            $table->string('post_color', 91)->nullable();
            $table->text('post_desc')->nullable();
            $table->string('post_title', 191)->nullable();
            $table->string('post_slug', 191)->nullable();
            $table->text('post_media')->nullable();
            $table->text('post_video')->nullable();
            $table->string('video_url', 191)->nullable();
            $table->timestamps();
        });
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        Schema::dropIfExists('posts');
    }
}
