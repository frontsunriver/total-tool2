<?php 


Route::group(['namespace'=>'Amcoders\Plugin\plan\http\controllers','middleware'=>['web','auth','admin'],'prefix'=>'admin/', 'as'=>'admin.'],function(){

	//here create your routes
	Route::resource('plan','PlanController');
    Route::post('plans/destroy','PlanController@destroy')->name('plans.destroy');
    Route::get('plan/payment/request','PlanController@payment')->name('plan.payment.request');
    Route::get('plan/payment/approved/{id}','PlanController@approved')->name('plan.payment.approved');
    Route::get('plan/payment/delete/{id}','PlanController@delete')->name('plan.payment.delete');
    Route::get('plan/payment/create','PlanController@payment_create')->name('plan.payment.create');
    Route::post('plan/user','PlanController@user')->name('plan.user');
    Route::post('plan/payment/create','PlanController@payment_store')->name('plan.payment.create');
    Route::post('plan/payment/update,{id}','PlanController@payment_update')->name('plan.payment.update');

    Route::get('/userplan/{id}','PlanController@UserPlan')->name('plan.userplan');

    
});

Route::group(['namespace'=>'Amcoders\Plugin\plan\http\controllers\Store','middleware'=>['web','auth','store','verified','approval'],'prefix'=>'store/', 'as'=>'store.'],function(){

	//here create your routes
	Route::get('plan','PlanController@index')->name('plan');
    Route::get('review','ReviewController@index')->name('review.index');
	Route::get('plan/checkout/{id}','PlanController@checkout')->name('plan.checkout');
	Route::post('create-payment','PlanController@payment')->name('plan.payment');
    Route::get('saas','PlanController@planCheck')->name('plancheck');

    Route::get('/payment/success','PlanController@success')->name('payment.success');
    Route::get('/payment/fail','PlanController@fail')->name('payment.fail');

    Route::get('logout',function(){
        \Auth::logout();
        return back();
    })->name('logout');

});


