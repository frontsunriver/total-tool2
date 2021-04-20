<?php 


Route::group(['as' =>'store.','prefix'=>'store','namespace'=>'Amcoders\Plugin\shop\http\controllers','middleware'=>['web','auth','store','verified','approval']],function(){

	Route::get('/my-media','ShopController@media')->name('media');
	Route::post('/media/destroy','ShopController@MediaDestroy')->name('medias.destroy');

	Route::resource('product','ShopController');
	Route::resource('addon-product','AddonProductController');
	Route::post('addon-products/destroy','AddonProductController@destroy')->name('addon-products.destroy');
	Route::post('products/destroy','ShopController@destroy')->name('products.destroy');
	Route::get('my-day','ShopController@day')->name('day.show');
	Route::post('shop-day','ShopController@updateday')->name('day.update');

	Route::get('/settings/my-information','ShopController@information')->name('my.information');
	Route::get('/settings/payouts','StatementController@payouts')->name('payouts');
	Route::get('/settings/payouts/setup','StatementController@setup')->name('payout.edit');
	Route::post('/payouts/setup/paypal','StatementController@paypalSetup')->name('payout.paypal');
	Route::post('/payouts/setup/bank','StatementController@bankSetup')->name('payout.bank');

	Route::post('withdraw','StatementController@withdraw')->name('withdraw');

	Route::post('information-update','ShopController@informationupdate')->name('my.information.update');
	Route::resource('coupon','CouponController');
	Route::post('coupons/destroy','CouponController@destroy')->name('coupons.destroy');
	Route::post('subscribe','ShopController@subscribe')->name('subscribe');

	Route::resource('order','OrderController');
	Route::get('order/invoice/{id}','OrderController@invoice')->name('invoice');
	Route::get('order/invoice/print/{id}','OrderController@invoice_print')->name('invoice_print');
	Route::get('/settings/earnings','StatementController@Earning')->name('earnings');

	Route::resource('menu','MenuController');
	Route::post('mendestroy','MenuController@destroy')->name('menu.des');

});



Route::group(['namespace'=>'Amcoders\Plugin\shop\http\controllers','middleware'=>['web','auth','admin'],'prefix'=>'admin/', 'as'=>'admin.'],function(){

	Route::get('/resturents','UserController@vendors')->name('vendor.index');
	Route::get('/resturents/requests','UserController@requests')->name('vendor.requests');
	Route::post('/ridersupdate/{id}','UserController@riderUpdate')->name('rider.update');

	Route::get('/rider','UserController@riders')->name('rider.index');
	Route::get('/rider/requests','UserController@riderrequests')->name('rider.requests');
	
	Route::get('/user/{id}','UserController@show')->name('vendor.show');
	Route::post('/userdestroy','UserController@UsersDelete')->name('vendor.destroys');
	Route::post('/resturentsupdate/{id}','UserController@sellerUpdate')->name('vendor.update');
	Route::get('/signalremove/{id}','UserController@signalremove')->name('signal.remove');

	Route::get('/payout/request','PayoutController@request')->name('payout.request');
	Route::get('/payout/history','PayoutController@history')->name('payout.history');
	Route::get('/payout/accounts','PayoutController@accounts')->name('payout.accounts');
	Route::get('/payout/{id}','PayoutController@show')->name('payout.show');
	Route::post('/payoutupdate/{id}','PayoutController@payoutUpdate')->name('payout.update');
	Route::get('/payout/destroy/{id}','PayoutController@destroy')->name('payout.destroy');

	Route::get('/customers','UserController@customers')->name('customer.index');

});


