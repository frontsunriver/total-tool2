<?php 



Route::group(['namespace'=>'Amcoders\Theme\khana\http\controllers','middleware'=>'web'],function(){

	Route::get('/','WelcomeController@index')->name('welcome');
	Route::post('send-contact','WelcomeController@contact')->name('contact.send');
	Route::get('/page/{slug}','WelcomeController@page');
	Route::get('/resturents','AreaController@resturents')->name('resturents.search');
	Route::get('/area/{slug}','AreaController@index')->name('area');
	Route::get('/areainfo/{id}','AreaController@show')->name('area.show');
	Route::get('/areadata/{id}','AreaController@areainfo');
	Route::get('locationinfo/{id}','AreaController@info')->name('location.restaurant.info');
	Route::get('/category/{slug}','AreaController@category')->name('category');
	Route::get('/store/{slug}','StoreController@show')->name('store.show');
	Route::get('/store_data/{slug}','StoreController@store_data')->name('store_data');

	Route::get('/addon_product','StoreController@addon_product')->name('addon_product');
	Route::post('addonproduct/add_to_cart','CartController@addon_add_to_cart')->name('addonproduct.add_to_cart');

	Route::get('add_to_cart','CartController@add_to_cart')->name('add_to_cart');

	Route::post('coupon','CartController@discount')->name('coupon');

	Route::get('cart/update','CartController@update')->name('cart.update');

	Route::get('cart/delete','CartController@delete')->name('cart.delete');

	Route::get('/get-top-rated-resturent','WelcomeController@topresturent')->name('top-resurent');
	Route::get('resturantinfo','StoreController@resturantinfo')->name('store.resturantinfo');

	Route::get('gallery','GalleryController@store_gallery')->name('store.gallery');

	Route::post('book/{slug}','BookController@store')->name('book.store');

	Route::get('checkout','CheckoutController@index')->name('checkout.index');

	Route::get('checkout/type','CheckoutController@type')->name('checkout.type');

	Route::get('user/login','LoginController@login')->name('user.login');
	Route::post('user/register','RegisterController@user_store')->name('user.register');	
	Route::get('user/register','RegisterController@register')->name('user.register');	
	Route::post('create-order','CheckoutController@CreateOrder')->name('order.store');

	Route::get('order/confirmation','CheckoutController@confirmation')->name('order.confirmation');
	Route::get('restaurant/register','Restaurant\RegisterController@index')->name('restaurant.register');
	Route::get('restaurant/register/step/2','Restaurant\RegisterController@step_2')->name('restaurant.register_step_2');
	Route::post('restaurant/register/step/2','Restaurant\RegisterController@step_2_store')->name('restaurant.register_step_2');
	Route::get('restaurant/register/step/3','Restaurant\RegisterController@step_3')->name('restaurant.register_step_3');
	Route::post('restaurant/register/step/3','Restaurant\RegisterController@step_3_store')->name('restaurant.register_step_3');
	Route::get('restaurant/register/step/4','Restaurant\RegisterController@step_4')->name('restaurant.register_step_4');
	Route::post('restaurant/register','Restaurant\RegisterController@store')->name('restaurant.register');

	Route::get('contact',function(){
		return view('theme::contact.index');
	});
});

Route::group(['namespace'=>'Amcoders\Theme\khana\http\controllers','middleware'=>['web','auth']],function(){
	Route::post('order/store','OrderController@store')->name('order.create');
	Route::post('stripe-charge','OrderController@stripe')->name('stripe.charge');

	Route::get('payment-with/stripe', 'OrderController@stripe_view');
	Route::get('payment-with/razorpay', 'OrderController@razorpay_view');
	Route::get('payment-success', 'OrderController@payment_success')->name('payment.success');

	Route::get('payment-fail', 'OrderController@payment_fail')->name('payment.fail');
});

Route::group(['namespace'=>'Amcoders\Theme\khana\http\controllers\Admin','middleware'=>['web','auth','admin'],'prefix'=>'admin/', 'as'=>'admin.'],function(){

	Route::resource('theme-option','WelcomeController');
	Route::get('order','OrderController@index')->name('order.index');
	Route::get('order/date/filter','OrderController@date_filter')->name('order.date.filter');
	Route::get('order/{id}','OrderController@details')->name('order.details');
	Route::post('orderupdate/{id}','OrderController@update')->name('order.update');
	Route::get('order/decline/{id}','OrderController@decline')->name('order.decline');
	Route::get('order/pickup/{id}','OrderController@pickup')->name('order.pickup');
	Route::get('order/delivery/{id}','OrderController@delivery')->name('order.delivery');
	Route::get('order/delete/{id}','OrderController@delete')->name('order.delete');
	Route::post('order/city/filter','OrderController@city')->name('order.city');
	Route::get('order/type/{type}','OrderController@type')->name('order.type');
	Route::get('review','ReviewController@index')->name('review');
	Route::get('review/delete/{id}','ReviewController@delete')->name('review.delete');

	Route::get('/theme-settings','SettingsController@index')->name('theme.setting');
	Route::post('/settings/update','SettingsController@update')->name('theme.setting.update');
});	


Route::group(['as' =>'rider.','prefix'=>'rider','namespace'=>'Amcoders\Theme\khana\http\controllers\Rider','middleware'=>['web','auth','rider','verified','riderapproval']],function(){
	// dashboard route
	Route::get('dashboard','DashboardController@dashboard')->name('dashboard');	
	Route::get('live/orders','OrderController@live_order')->name('live.order');
	Route::get('order/{id}','OrderController@details')->name('order.details');
	Route::get('order/decline/{id}','OrderController@decline')->name('order.decline');
	Route::get('order/pickup/{id}','OrderController@pickup')->name('order.pickup');
	Route::get('orders/latest','OrderController@jsonOrders')->name('order.latest');
	Route::post('order/delivery/{id}','OrderController@delivery')->name('order.delivery');
	Route::get('order/locationmap/{id}','OrderController@map')->name('order.map');
	Route::get('/orders','OrderController@index')->name('orders');
	Route::post('order/live','OrderController@live')->name('order.live');

	Route::get('/earnings','StatementController@earnings')->name('earnings');
	Route::get('/payouts','StatementController@payout')->name('payouts');
	Route::get('/setup','StatementController@setup')->name('payout.edit');
	Route::post('paypalsetup','StatementController@paypalSetup')->name('payout.paypal');
	Route::post('banksetup','StatementController@bankSetup')->name('payout.bank');
	Route::post('withdraw','StatementController@withdraw')->name('withdraw');

	Route::post('subscribe','RiderController@subscribe')->name('subscribe');
	Route::get('/settings/my-information','RiderController@information')->name('my.information');
	Route::post('information-update','RiderController@informationupdate')->name('my.information.update');

	Route::post('status','RiderController@status')->name('status');

});





Route::group(['as' =>'rider.','prefix'=>'rider','namespace'=>'Amcoders\Theme\khana\http\controllers\Rider','middleware'=>['web']],function(){
	// dashboard route
	Route::get('register','RegisterController@register')->name('register');	
	Route::post('register','RegisterController@store')->name('register');	
	Route::get('login','LoginController@login')->name('login');	
	Route::get('approval',function(){
		return view('theme::rider.approval');
	})->name('approval');
});


Route::group(['as' =>'author.','prefix'=>'author','namespace'=>'Amcoders\Theme\khana\http\controllers\Author','middleware'=>['web','auth']],function(){
	// dashboard route
	Route::get('dashboard','DashboardController@dashboard')->name('dashboard');
	Route::post('settings/update','SettingsController@update')->name('settings.update');
	Route::post('review','RattingController@review')->name('review');
	Route::get('order/{id}','OrderController@details')->name('order.details');
	
});


Route::get('header/notify','Amcoders\Theme\khana\http\controllers\WelcomeController@notify')->name('header.notify')->middleware('web');
