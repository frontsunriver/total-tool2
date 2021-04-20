<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class CreateAddonsTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('addons', function (Blueprint $table) {
            //$table->id();
            $table->unsignedBigInteger('term_id');
            $table->unsignedBigInteger('addon_id');
            
            $table->timestamps();
            $table->foreign('term_id')
            ->references('id')->on('terms')
            ->onDelete('cascade');

            $table->foreign('addon_id')
            ->references('id')->on('terms')
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
        Schema::dropIfExists('addons');
    }
}
