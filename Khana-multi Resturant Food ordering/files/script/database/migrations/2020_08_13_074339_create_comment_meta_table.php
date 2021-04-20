<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class CreateCommentMetaTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('comment_meta', function (Blueprint $table) {
          $table->id();
          $table->unsignedBigInteger('comment_id');
          $table->unsignedBigInteger('vendor_id');
          $table->integer('star_rate')->default(0);
          $table->text('comment')->nullable();
          $table->timestamps();
          $table->foreign('comment_id')
                 ->references('id')->on('comments')
                 ->onDelete('cascade');
          $table->foreign('vendor_id')
                ->references('id')->on('users')
                ->onDelete('cascade');
       });
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        Schema::dropIfExists('comment_meta');
    }
}
