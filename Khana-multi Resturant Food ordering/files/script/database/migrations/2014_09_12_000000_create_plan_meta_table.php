<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class CreatePlanMetaTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('plan_meta', function (Blueprint $table) {
            $table->id();
            $table->string('name');
            $table->integer('s_price');
            $table->string('duration')->deafult('month');
            $table->integer('f_resturent')->default(0);
            $table->integer('table_book')->default(0);
            $table->integer('img_limit')->default(0);
            $table->integer('commission')->default(0);
            $table->integer('status')->default(1);
           
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
        Schema::dropIfExists('plan_meta');
    }
}
