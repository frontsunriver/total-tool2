<?php 
function RegisterAdminMenuBar(){

	if(Auth::User()->role->id == 1){
		if (Auth()->user()->can('dashboard')) {
			$data['admin_dashboard']=array(
				'name' => __('Dashboard'),
				'active' => Request::is('admin/dashboard'),
				'icon' => 'fas fa-tachometer-alt',
				'url' => url('admin/dashboard')
			);
		}

		if (Auth()->user()->can('media.list')) {
			$data['admin_media']=array(
				'name' => __('Media'),
				'active' => Request::is('admin/media'),
				'icon' => 'far fa-images',
				'url' => route('admin.media.index')
			);
		}

		if (Auth()->user()->can('page.list')) {
			$data['admin_pages']=array(
				'name' => __('Pages'),
				'active' => Request::is('admin/page*'),
				'icon' => 'far fa-copy',
				'url' => route('admin.page.index')
			);
		}

		

		if (Amcoders\Plugin\Plugin::is_active('product')) {
			if (Auth()->user()->can('product.list')) {
				$products[__('Products')] = route('admin.product.index');
			}
			if (Auth()->user()->can('product.category')) {
				$products[__('Menu Category')] =  route('admin.shop.category.index','type=1');
				$products[__('Cuisine Category')] =  route('admin.shop.category.index','type=2');

			}

			if (count($products ?? []) > 0) {
				$data['shop_set']=array(
					'name' => __('Products'),
					'icon' => 'fas fa-shopping-cart',
					'child'=> $products
				);
			}
		}

		if (Amcoders\Plugin\Plugin::is_active('shop')) {
			if (Auth()->user()->can('resturents.requests')) {
				$vendors[__('Resturents Requests')] = route('admin.vendor.requests');
			}
			if (Auth()->user()->can('all.resturents')) {
				$vendors[__('All Resturents')] = route('admin.vendor.index');
			}
			if (Auth()->user()->can('manage.review')) {
				$vendors[__('Manage Review')] = route('admin.review');
			}
			if (count($vendors ?? []) > 0) {
				$data['vendors']=array(
					'name' => __('Restaurant'),
					'icon' => 'fas fa-utensils',
					'active'=>Request::is('admin/resturents*'),
					'child'=> $vendors
				);
			}

			if (Auth()->user()->can('rider.request')) {
				$riders[__('Rider Requests')] = route('admin.rider.requests');
			}
			if (Auth()->user()->can('all.rider')) {
				$riders[__('All Riders')] = route('admin.rider.index');
			}
			if (count($riders ?? []) > 0) {
				$data['riders']=array(
					'name' => __('Riders'),
					'icon' => 'fas fa-bicycle',
					'active'=>Request::is('admin/rider*'),
					'child'=> $riders	
				);
			}

			if (Auth()->user()->can('customer.list')) {
				$data['customers']=array(
					'name' => __('Customers'),
					'icon' => 'fas fa-users',
					'url' => route('admin.customer.index')
				);
			}

			if (Auth()->user()->can('payout.request')) {
				$payouts[__('Payments Request')] = route('admin.payout.request');
			}
			if (Auth()->user()->can('payout.history')) {
				$payouts[__('Payments History')] = route('admin.payout.history');
			}
			if (Auth()->user()->can('payout.account')) {
				$payouts[__('Payout Accounts')] = route('admin.payout.accounts');
			}

			if (count($payouts ?? []) > 0) {
				$data['payouts']=array(
					'name' => __('Payouts'),
					'icon' => 'fas fa-wallet',
					'active'=>Request::is('admin/payout*'),
					'child'=> $payouts
				);
			}


		}



		if (Auth()->user()->can('order.list')) {
			$orders[__('All Orders')] = route('admin.order.index');
		}
		if (Auth()->user()->can('order.control')) {
			$orders[__('Pending Orders')] = route('admin.order.type','pending');
			$orders[__('Accept Orders')] = route('admin.order.type','accepted');
			$orders[__('Complete Orders')] = route('admin.order.type','complete');
		}

		if (count($orders ?? []) > 0) {
			$data['shop_order']=array(
				'name' => __('Orders'),
				'icon' => 'fas fa-cubes',
				'active'=>Request::is('admin/order*'),
				'child'=> $orders
			);
		}


		if (Amcoders\Plugin\Plugin::is_active('plan')) {
			if (Auth()->user()->can('plan.create')) {
				$plans[__('Create New')] = route('admin.plan.create');
			}
			if (Auth()->user()->can('plan.list')) {
				$plans[__('Manage Plan')] = route('admin.plan.index');
			}
			if (Auth()->user()->can('payment.request')) {
				$plans[__('Payment Request')] = route('admin.plan.payment.request');
			}

			if (count($plans ?? []) > 0) {
				$data['plan']=array(
					'name' => __('Plans'),
					'icon' => 'fas fa-money-check-alt',
					'child'=> $plans
				);
			}
		}


		if (Amcoders\Plugin\Plugin::is_active('locations')) {
			if (Auth()->user()->can('location.create')) {
				$locations[__('Create New')] = route('admin.location.create');
			}

			if (Auth()->user()->can('location.list')) {
				$locations[__('Manage Locations')] = route('admin.location.index');
			}

			if (count($locations ?? []) > 0) {
				$data['location']=array(
					'name' => __('Locations'),
					'icon' => 'fas fa-map-marked-alt',
					'child'=> $locations
				);
			}
		}
		if (Amcoders\Plugin\Plugin::is_active('locations')) {
			if (Auth()->user()->can('badge.control')) {
				$data['badges']=array(
					'name' => __('Badges'),
					'icon' => 'fas fa-certificate',
					'child'=> array(
						__('Seller Badge') => route('admin.badge.index','type=seller'),

					)
				);
			}
		}

		if (Amcoders\Plugin\Plugin::is_active('locations')) {

			if (Auth()->user()->can('featured.control')) {
				$data['featured']=array(
					'name' => __('Featured'),
					'icon' => 'fas fa-highlighter',
					'child'=> array(
						__('Featured Seller') => route('admin.featured.create','type=seller'),
						__('Manage Featured Seller') => route('admin.featured.create','st=1&type=seller&manage=featured_seller'),

					)
				);
			}
		}

		if (Amcoders\Plugin\Plugin::is_active('product')) {
			if (Auth()->user()->can('earning.order.report')) {
				$report[__('Earning By Order')] = route('admin.earning.index');
			}

			if (Auth()->user()->can('earning.delivery.report')) {
				$report[__('Earning By Delivery')] = route('admin.earning.delivery');
			}

			if (Auth()->user()->can('earning.subscription.report')) {
				$report[__('Earning By Subscription')] = route('admin.earning.saas');
			}

			if (count($report ?? []) > 0) {
				$data['report']=array(
					'name' => __('Earning Reports'),
					'icon' => 'fas fa-money-bill-alt',
					'active'=>Request::is('admin/earnings*'),
					'child'=> $report,		

				);
			}
		}

		if (Auth()->user()->can('theme')) {
			$appearance[__('Theme')] = route('admin.theme.index');
		}
		if (Auth()->user()->can('menu')) {
			$appearance[__('Menu')] = route('admin.menu.index');
		}

		if (count($appearance ?? []) > 0) {
			$data['admin_Appearance']=array(

				'name' => __('Appearance'),
				'active' => Request::is('admin/appearance*'),
				'icon' => 'fas fa-palette',
				'child'=> $appearance
			);
		}

		// if (Auth()->user()->can('plugin.control')) {
		// 	$data['admin_plugin']=array(
		// 		'name' => __('Plugins'),
		// 		'active' => Request::is('admin/plugin'),
		// 		'icon' => 'fas fa-puzzle-piece',
		// 		'url' => route('admin.plugin.index')
		// 	);
		// }

		if (Auth()->user()->can('site.settings')) {
			$settings[__('Site Settings')] = route('admin.theme.setting');
		}

		if (Auth()->user()->can('seo')) {
			$settings[__('Seo')] = route('admin.seo.index');
		}
		if (Auth()->user()->can('file.system')) {
			$settings[__('Filesystem')] = route('admin.filesystem.index');
		}
		if (Auth()->user()->can('system.settings')) {
			$settings[__('System Settings')] = route('admin.env.index');
		}

		if (Auth()->user()->can('payment.settings')) {
			if (Amcoders\Plugin\Plugin::is_active('Paymentgetway')) {
			$settings[__('Payment Settings')] = route('admin.payment.settings');
		    }
		}

		if (count($settings ?? []) > 0) {
			$data['admin_settings']=array(
				'name' => __('Settings'),
				'active' => Request::is('admin/setting*'),
				'icon' => 'fas fa-cogs',
				'child'=> $settings
			);
		}

		if (Auth()->user()->can('language.control')) {
			$data['admin_language']=array(
				'name' => __('Language'),
				'active' => Request::is('admin/language*'),
				'icon' => 'fas fa-language',
				'child'=> array(
					__('Create Language') => route('admin.language.create'),		
					__('Language Settings') => route('admin.language.index')
				)
			);
		}

		if (Auth()->user()->can('role.list')) {
			$admins[__('Roles')] = route('admin.role.index');
		}
		if (Auth()->user()->can('admin.list')) {
			$admins[__('Admins')] = route('admin.users.index');
		}
		if (count($admins ?? []) > 0) {
			$data['admin_roles']=array(
				'name' => __('Admins And Roles'),
				'active' => Request::is('admin/users*') || Request::is('admin/role*'),
				'icon' => 'fas fa-user-shield',
				'child'=> $admins
			);
		}

		

		if (Auth()->user()->can('update')) {
			if (Amcoders\Plugin\Plugin::is_active('update')) {
				$data['admin_update']=array(
					'name' => __('Update'),
					'active' => Request::is('admin/update'),
					'icon' => 'fas fa-upload',
					'url'=>route('admin.update.index')
				);
			}
		}


	}


	if(Auth::User()->role->id == 3){
		
		if (Amcoders\Plugin\Plugin::is_active('shop')) {
			$data['dashboard']=array(
				'name' => __('Dashboard'),
				'icon' => 'fas fa-tachometer-alt',
				'active' => Request::is('store/dashboard'), 
				'url' => route('store.dashboard')
			);

			$data['media']=array(
				'name' => __('Media'),
				'icon' => 'far fa-images',
				'active' => Request::is('/store/media'), 
				'url' => route('store.media')
			);
			$data['menu']=array(
				'name' => __('Category'),
				'icon' => 'fas fa-list-ul',
				'active' => Request::is('store/order'), 
				'url' => route('store.menu.index')
			);

			$data['order']=array(
				'name' => __('Live Orders'),
				'icon' => 'fas fa-th',
				'active' => Request::is('store/order'), 
				'url' => route('store.order.index','type=live')
			);
			$data['orders']=array(
				'name' => __('Orders'),
				'icon' => 'fas fa-cubes',
				'active' => Request::is('store/orders'), 
				'url' => route('store.order.index')
			);

			

			$data['Products']=array(
				'name' => __('Products'),
				'icon' => 'fas fa-pizza-slice',
				'active' => Request::is('store/product*'), 
				'child'=> array(
					__('Add New') => route('store.product.create'),
					__('Manage Products') => route('store.product.index'),			
					__('Addon Products') => route('store.addon-product.index')					

				)
			);

			if (Amcoders\Plugin\Plugin::is_active('qrmenu')) {
				$data['qrmenu']=array(
					'name' => __('QR Builder'),
					'icon' => 'fas fa-qrcode',
					'active' => Request::is('store/qrmenu*'), 
					'url' => route('store.qrmenu.index')
				);
			}


			$data['coupon']=array(
				'name' => __('Coupon'),
				'icon' => 'fa fa-gift',   
				'active' => Request::is('store/coupon'), 
				'url' => route('store.coupon.index')
			);

			$data['plan']=array(
				'name' => __('Manage Plan'),
				'icon' => 'fa fa-tasks',
				'active' => Request::is('store/plan'), 
				'url' => route('store.plan')
			);

			$data['review']=array(
				'name' => __('Manage Review'),
				'icon' => 'fa fa-star',
				'active' => Request::is('store/review'), 
				'url' => url('store/review')
			);


			$data['settings']=array(
				'name' => __('Seller Settings'),
				'icon' => 'fas fa-cogs',
				'active' => Request::is('store/settings*'), 
				'child'=> array(
					__('Shop Day') => route('store.day.show'),
					__('Earnings') => route('store.earnings'),					
					__('Payouts') => route('store.payouts'),			
					__('information') => route('store.my.information')
				)
			);         
			$data['store']=array(
				'name' => __('Shop'),
				'icon' => 'fas fa-store-alt',
				
				'url' => url('/store',Auth::user()->slug)
			);
			$data['logout']=array(
				'name' => __('Logout'),
				'icon' => 'fas fa-sign-out-alt',
				'active' => Request::is('/'), 
				'url' => route('store.logout')
			);

		}
	}


	if(Auth::User()->role->id == 4){
		$data['dashboard']=array(
			'name' => __('Dashboard'),
			'icon' => 'fas fa-tachometer-alt',
			'url' => route('rider.dashboard')
		);

		$data['liveorder']=array(
			'name' => __('Live Orders'),
			'icon' => 'fas fa-satellite',
			'url' => route('rider.live.order')
		);
		
		$data['Orders']=array(
			'name' => __('Orders'),
			'icon' => 'fas fa-cubes',
			'url' => route('rider.orders')
		);

		$data['Earnings']=array(
			'name' => __('Earnings'),
			'icon' => 'fas fa-money-bill',
			'url' => route('rider.earnings')
		);

		$data['payout']=array(
			'name' => __('Payouts'),
			'icon' => 'fas fa-hand-holding-usd',
			'url' => route('rider.payouts')
		);

		$data['settings']=array(
			'name' => __('Settings'),
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