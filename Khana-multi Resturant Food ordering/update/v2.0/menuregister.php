<?php 
function RegisterAdminMenuBar(){

	if(Auth::User()->role->id == 1){

		$data['admin_dashboard']=array(
			'name' => 'Dashboard',
			'active' => Request::is('admin/dashboard'),
			'icon' => 'fas fa-tachometer-alt',
			'url' => url('admin/dashboard')
		);

		$data['admin_media']=array(
			'name' => 'Media',
			'active' => Request::is('admin/media'),
			'icon' => 'far fa-images',
			'url' => route('admin.media.index')
		);

		$data['admin_pages']=array(
			'name' => 'Pages',
			'active' => Request::is('admin/page*'),
			'icon' => 'far fa-copy',
			'url' => route('admin.page.index')
		);

		// $data['admin_blogs']=array(
		// 	'name' => 'Blogs',
		// 	'active' => Request::is('admin/blog*'),
		// 	'icon' => 'fab fa-blogger-b',
		// 	'child'=> array(
		// 		'Post' => route('admin.post.index'),
		// 		'Categories' => route('admin.category.index'),
		// 		'Comments' => route('admin.comment.index'),				
		// 	)
		// );

		if (Amcoders\Plugin\Plugin::is_active('product')) {
		$data['shop_set']=array(
			'name' => 'Products',
			'icon' => 'fas fa-shopping-cart',
			'child'=> array(
				'Products' => route('admin.product.index'),
				'Menu Category' => route('admin.shop.category.index','type=1'),
				'Cuisine Category' => route('admin.shop.category.index','type=2'),
				
						
			)
		);
	   }

		if (Amcoders\Plugin\Plugin::is_active('shop')) {
		$data['vendors']=array(
			'name' => 'Restaurant',
			'icon' => 'fas fa-utensils',
			'active'=>Request::is('admin/resturents*'),
			'child'=> array(
				'Resturents Requests' => route('admin.vendor.requests'),
				'All Resturents' => route('admin.vendor.index'),
				'Manage Review' => route('admin.review'),		
				
			)
		);
	   
		$data['riders']=array(
			'name' => 'Riders',
			'icon' => 'fas fa-bicycle',
			'active'=>Request::is('admin/rider*'),
			'child'=> array(
				'Rider Requests' => route('admin.rider.requests'),
				'All Riders' => route('admin.rider.index'),		
				
			)
		);
		$data['customers']=array(
			'name' => 'Customers',
			'icon' => 'fas fa-users',
			 'url' => route('admin.customer.index')
		);

		$data['payouts']=array(
			'name' => 'Payouts',
			'icon' => 'fas fa-wallet',
			'active'=>Request::is('admin/payout*'),
			'child'=> array(
				'Payments Request' => route('admin.payout.request'),
				'Payments History' => route('admin.payout.history'),
				'Payout Accounts' => route('admin.payout.accounts'),

			)
		);
	 }

		$data['shop_order']=array(
			'name' => 'Orders',
			'icon' => 'fas fa-cubes',
			'child'=> array(
				'All Orders' => route('admin.order.index'),				
				'Pending Orders' => route('admin.order.type','pending'),				
				'Accept Orders' => route('admin.order.type','accepted'),				
				'Complete Orders' => route('admin.order.type','complete'),				
			)
		);


		if (Amcoders\Plugin\Plugin::is_active('plan')) {
			$data['plan']=array(
				'name' => 'Plans',
				'icon' => 'fas fa-money-check-alt',
				'child'=> array(
					'Create New' => route('admin.plan.create'),
					'Manage Plan' => route('admin.plan.index'),				
					'Payment Request' => route('admin.plan.payment.request')				
				)
			);
		}


		if (Amcoders\Plugin\Plugin::is_active('locations')) {
			$data['location']=array(
				'name' => 'Locations',
				'icon' => 'fas fa-map-marked-alt',
				'child'=> array(
					'Create New' => route('admin.location.create'),
					'Manage Locations' => route('admin.location.index')				
				)
			);
		}
		if (Amcoders\Plugin\Plugin::is_active('locations')) {
			$data['badges']=array(
				'name' => 'Badges',
				'icon' => 'fas fa-certificate',
				'child'=> array(
					'Seller Badge' => route('admin.badge.index','type=seller'),
					//'Rider Badge' => route('admin.badge.index','type=rider'),				
					//'Customer Badge' => route('admin.badge.index','type=customer')				
				)
			);
		}

		if (Amcoders\Plugin\Plugin::is_active('locations')) {
		
		$data['featured']=array(
			'name' => 'Featured',
			'icon' => 'fas fa-highlighter',
			'child'=> array(
				'Featured Seller' => route('admin.featured.create','type=seller'),
				'Manage Featured Seller' => route('admin.featured.create','st=1&type=seller&manage=featured_seller'),
				//'Featured Rider' => route('admin.featured.create','type=rider'),	
				//'Featured Customer' => route('admin.featured.create','type=customer')				
			)
		);
	   }

	   if (Amcoders\Plugin\Plugin::is_active('product')) {
	   $data['report']=array(
			'name' => 'Earning Reports',
			'icon' => 'fas fa-money-bill-alt',
			'active'=>Request::is('admin/earnings*'),
			'child'=> array(
				'Earning By Order' => route('admin.earning.index'),
				'Earning By Delivery' => route('admin.earning.delivery'),		
				'Earning By Subscription' => route('admin.earning.saas'),		
				
			)
		);
	   }

		$data['admin_Appearance']=array(
			'name' => 'Appearance',
			'active' => Request::is('admin/appearance*'),
			'icon' => 'fas fa-palette',
			'child'=> array(
				'Theme' => route('admin.theme.index'),
				'Menu' => route('admin.menu.index'),
				//'Custom css js' => route('admin.script.index'),

			)
		);

		$data['admin_plugin']=array(
			'name' => 'Plugins',
			'active' => Request::is('admin/plugin'),
			'icon' => 'fas fa-puzzle-piece',
			'url' => route('admin.plugin.index')
		);

		$data['admin_settings']=array(
			'name' => 'Settings',
			'active' => Request::is('admin/setting*'),
			'icon' => 'fas fa-cogs',
			'child'=> array(
				'Site Settings' => route('admin.theme.setting'),
				'Seo' => route('admin.seo.index'),							
				'Filesystem' => route('admin.filesystem.index'),		
				'System Settings' => route('admin.env.index'),
			)
		);

		$data['admin_language']=array(
			'name' => 'Language',
			'active' => Request::is('admin/setting*'),
			'icon' => 'fas fa-language',
			'child'=> array(
				'Create Language' => route('admin.language.create'),		
				'Language Settings' => route('admin.language.index')
			)
		);

		if (Amcoders\Plugin\Plugin::is_active('update')) {
		$data['admin_update']=array(
			'name' => 'Update',
			'active' => Request::is('admin/update'),
			'icon' => 'fas fa-upload',
			'url'=>route('admin.update.index')
		);
	   }



	}


	if(Auth::User()->role->id == 3){
		
		if (Amcoders\Plugin\Plugin::is_active('shop')) {
			$data['dashboard']=array(
				'name' => 'Dashboard',
				'icon' => 'fas fa-tachometer-alt',
				'active' => Request::is('store/dashboard'), 
				'url' => route('store.dashboard')
			);

			$data['media']=array(
				'name' => 'Media',
				'icon' => 'far fa-images',
				'active' => Request::is('/store/media'), 
				'url' => route('store.media')
			);
			$data['menu']=array(
				'name' => 'Category',
				'icon' => 'fas fa-list-ul',
				'active' => Request::is('store/order'), 
				'url' => route('store.menu.index')
			);

			$data['order']=array(
				'name' => 'Live Orders',
				'icon' => 'fas fa-th',
				'active' => Request::is('store/order'), 
				'url' => route('store.order.index','type=live')
			);
			$data['orders']=array(
				'name' => 'Orders',
				'icon' => 'fas fa-cubes',
				'active' => Request::is('store/orders'), 
				'url' => route('store.order.index')
			);

			

			$data['Products']=array(
				'name' => 'Products',
				'icon' => 'fas fa-pizza-slice',
				'active' => Request::is('store/product*'), 
				'child'=> array(
					'Add New' => route('store.product.create'),
					'Manage Products' => route('store.product.index'),			
					'Addon Products' => route('store.addon-product.index')					

				)
			);

			$data['coupon']=array(
				'name' => 'Coupon',
				'icon' => 'fa fa-gift',
				'active' => Request::is('store/coupon'), 
				'url' => route('store.coupon.index')
			);

			$data['plan']=array(
				'name' => 'Manage Plan',
				'icon' => 'fa fa-tasks',
				'active' => Request::is('store/plan'), 
				'url' => route('store.plan')
			);

			$data['review']=array(
				'name' => 'Manage Review',
				'icon' => 'fa fa-star',
				'active' => Request::is('store/review'), 
				'url' => url('store/review')
			);


			$data['settings']=array(
				'name' => 'Seller Settings',
				'icon' => 'fas fa-cogs',
				'active' => Request::is('store/settings*'), 
				'child'=> array(
					'Shop Day' => route('store.day.show'),
					'Earnings' => route('store.earnings'),					
					'Payouts' => route('store.payouts'),			
					'information' => route('store.my.information')					
				)
			);         
			$data['store']=array(
				'name' => 'Shop',
				'icon' => 'fas fa-store-alt',
				
				'url' => url('/store',Auth::user()->slug)
			);
			$data['logout']=array(
				'name' => 'Logout',
				'icon' => 'fas fa-sign-out-alt',
				'active' => Request::is('/'), 
				'url' => route('store.logout')
			);

		}
	}


	if(Auth::User()->role->id == 4){
		$data['dashboard']=array(
			'name' => 'Dashboard',
			'icon' => 'fas fa-tachometer-alt',
			'url' => route('rider.dashboard')
		);

		$data['liveorder']=array(
			'name' => 'Live Orders',
			'icon' => 'fas fa-satellite',
			'url' => route('rider.live.order')
		);
		
		$data['Orders']=array(
			'name' => 'Orders',
			'icon' => 'fas fa-cubes',
			'url' => route('rider.orders')
		);

		$data['Earnings']=array(
			'name' => 'Earnings',
			'icon' => 'fas fa-money-bill',
			'url' => route('rider.earnings')
		);

		$data['payout']=array(
			'name' => 'Payouts',
			'icon' => 'fas fa-hand-holding-usd',
			'url' => route('rider.payouts')
		);

		$data['settings']=array(
			'name' => 'Settings',
			'icon' => 'fas fa-user-cog',
			'url' => route('rider.my.information')	
		);    


	}
	

	/* if u want to use only url without collapse follow this  demo*/

	// $data['key']=array(
	// 	'name' => 'name',
	// 	'icon' => 'icon name',
	// 	'url' => 'url/param'
	// );

	return $data ?? [];
}