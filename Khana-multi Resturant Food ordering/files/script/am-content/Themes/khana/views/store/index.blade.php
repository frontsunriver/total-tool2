@extends('theme::layouts.app')

@push('css')
<link rel="stylesheet" href="{{ theme_asset('khana/public/css/bootstrap-datetimepicker.min.css') }}">
<style>
	.owl-nav {
		display: none;
	}
</style>
@endpush

@section('content')
@php
$currency=\App\Options::where('key','currency_name')->select('value')->first();
@endphp
<!-- success-alert start -->
<div class="alert-message-area">
	<div class="alert-content">
		<h4 class="ale">{{ __('Your Settings Successfully Updated') }}</h4>
	</div>
</div>
<!-- success-alert end -->

<!-- error-alert start -->
<div class="error-message-area">
	<div class="error-content">
		<h4 class="error-msg"></h4>
	</div>
</div>
<!-- error-alert end -->

<!-- modal area start -->
<section>
	<div class="modal-area d-none">
		<div class="modal-main-content">

		</div>
	</div>
</section>
<!-- modal area end -->
<input type="hidden" id="gallery_url" value="{{ route('store.gallery') }}">
<section>
	<div class="store-area store_fixed">
		<div class="container-fluid">
			<div class="row">
				<div class="col-lg-9 p-0">
					<div class="store-left-section">
						<div class="store-banner-img">
							<img src="{{ asset($store->preview->content) }}" alt="">
							<div class="conatiner">
								<div class="badge-area">
									<img src="{{ $store->badge->preview->content ?? null }}" alt="">
								</div>
								<div class="row">
									<div class="col-lg-8 p-0">
										<div class="store-info">
											<div class="store-logo d-flex">
												<img src="{{ asset($store->avatar) }}" alt="">
												<div class="store-another-content d-block">
													<div class="d-flex">
														<h3>{{ $store->name }}</h3> <span class="badge badge-red">{{ $store->status == 'offline' ? 'closed' : 'Open' }}</span>
													</div>
													@php 
													$json_info = json_decode($store->info->content);
													@endphp
													<p class="first"><i class="fas fa-map-marker-alt"></i> {{ $json_info->full_address }}</p>
													<p><i class="fas fa-map-pin"></i> {{ $json_info->address_line }}</p>

												</div>
											</div>
										</div>
									</div>
									<div class="col-lg-4 p-0">
										<div class="store-main-info f-right">
											<div class="store-avg-time">
												<div class="delivery-ag-time">
													<i class="far fa-clock"></i>
													<span>{{ $store->delivery->content }} {{ __('MIN') }}</span>
												</div>
											</div>
											<div class="store-rating-area d-flex">
												<div class="total-avg-rating">
													<p><i class="fas fa-star"></i> {{ number_format($store->avg_ratting->content,1) }}</p>
												</div>
												<div class="total-rating">
													<p>{{ $store->ratting->content }} {{ __('Ratings') }}</p>
												</div>
											</div>
										</div>
									</div>
								</div>
							</div>
						</div>
						<div class="store-action {{ $store->status == 'offline' ? 'offline' : '' }}">
							<div class="store-inline-action">
								<nav>
									<ul class="nav nav-tabs">
										<li class="active"><a href="#online_order" data-toggle="tab" class="active">{{ __('Online Order') }}</a></li>
										<li><a href="#gallery" data-toggle="tab">{{ __('Gallery') }}</a></li>
										<li><a class="restutant_info" onclick="restaurantsinfo('{{ $store->slug }}')" href="#info" data-toggle="tab">{{ __('Restaurant Info') }}</a></li>
										<input type="hidden" id="resturantinfo_url" value="{{ route('store.resturantinfo') }}">
										@if($store->usersaas->table_book==1)
										<li><a href="#book" data-toggle="tab">{{ __('Book A Table') }}</a></li>
										@endif
										<li><a href="#rating" data-toggle="tab">{{ __('Rating & Reviews') }}</a></li>
									</ul>
								</nav>
							</div>
						</div>
						<div class="tab-content">
							<div class="online-food tab-pane fade in active show" id="online_order">
								<div class="loader-main-area">
									<div class="loader-area">
										<div class="loader"></div>
									</div>
								</div>
							</div>
							<input type="hidden" id="add_to_cart_url" value="{{ route('add_to_cart') }}">
							<input type="hidden" id="cart_update" value="{{ route('cart.update') }}">
							<input type="hidden" id="cart_delete" value="{{ route('cart.delete') }}">
							<div class="online-food tab-pane fade" id="gallery">
								<div class="single-category-food mt-50 mb-50">
									<div class="single-category-main-content">
										<div class="owl-carousel gallery">
											@foreach($galleries as $gallery)
											<div class="col-lg-12">
												<img class="img-fluid" src="{{ asset($gallery) }}" alt="">
											</div>
											@endforeach
										</div>
									</div>
								</div>
							</div>
							<div class="online-food tab-pane fade" id="info">
								
							</div>
							<div class="online-food tab-pane fade" id="book">
								<div class="single-category-food mt-50 mb-50">
									<div class="single-category-main-content">
										<div class="row mb-20">
											<div class="col-lg-6">
												<h3>{{ __('Book A Table') }}</h3>
											</div>
										</div>
										<div class="row">
											<div class="col-lg-12">
												<div class="group-section">
													<form action="{{ route('book.store',$store->slug) }}" method="POST" id="book_form">
														@csrf
														<div class="book-table-section">
															<h5 class="mb-15">{{ __('Booking Information') }}</h5>
															<div class="form-group">
																<label>{{ __('Number of Guests') }}</label>
																<input type="text" class="form-control" name="number_of_gutes" placeholder="Number of Guests">
															</div>
															<div class="form-group">
																<label>{{ __('Date Of Booking') }}</label>
																<input type="text" class="form-control" id="date" name="date" placeholder="Date Of Booking" autocomplete="off">
															</div>
															<h5 class="mb-15 mt-30">{{ __('Contact Information') }}</h5>
															<div class="form-group">
																<label>{{ __('Name') }}</label>
																<input type="text" class="form-control" name="name" placeholder="{{ __('Name') }}">
															</div>
															<div class="form-group">
																<label>{{ __('Email') }}</label>
																<input type="email" class="form-control" name="email" placeholder="{{ __('Email') }}">
															</div>
															<div class="form-group">
																<label>{{ __('Phone Number') }}</label>
																<input type="text" class="form-control" name="mobile" placeholder="{{ __('Phone Number') }}">
															</div>
															<div class="form-group">
																<label>{{ __('Your Instructions') }}</label>
																<textarea class="form-control" name="message"></textarea>
															</div>
															<div class="button-area">
																<button id="book_submit" type="submit">{{ __('Submit') }}</button>
															</div>
														</div>
													</form>
												</div>
											</div>
										</div>
									</div>
								</div>
							</div>
							<div class="online-food tab-pane fade" id="rating">
								<div class="single-category-food mt-50 mb-50">
									<div class="single-category-main-content">
										<div class="row">
											<div class="col-lg-6">
												<h3>{{ __('Rating & Reviews') }}</h3>
											</div>
										</div>
										<div class="row">
											<div class="col-lg-12">
												<div class="room-review pt-30">
													<div class="review-rate">
														<div class="row">
															<div class="col-lg-4">
																<div class="room-review-count text-center">
																	<p>{{ number_format($store->avg_ratting->content,1) }}</p>
																</div>
															</div>
															<div class="col-lg-8">
																<div class="review-bar">
																	<div class="single-progress-bar">
																		<div class="progressbar-label">
																			<h5>{{ __('5 Star') }} <span class="f-right">{{ $five_rattings }}%</span></h5>
																			<div class="progress">
																				<div class="progress-bar w-{{ $five_rattings }}" role="progressbar" aria-valuenow="{{ $five_rattings }}" aria-valuemin="0" aria-valuemax="100"></div>
																			</div>
																		</div>
																	</div>
																	<div class="single-progress-bar">
																		<div class="progressbar-label">
																			<h5>{{ __('4 Star') }} <span class="f-right">{{ $four_rattings }}%</span></h5>
																			<div class="progress">
																				<div class="progress-bar w-{{ $four_rattings }}" role="progressbar" aria-valuenow="{{ $four_rattings }}" aria-valuemin="0" aria-valuemax="100"></div>
																			</div>
																		</div>
																	</div>
																	<div class="single-progress-bar">
																		<div class="progressbar-label">
																			<h5>{{ __('3 Star') }} <span class="f-right">{{ $three_rattings }}%</span></h5>
																			<div class="progress">
																				<div class="progress-bar w-{{ $three_rattings }}" role="progressbar" aria-valuenow="{{ $three_rattings }}" aria-valuemin="0" aria-valuemax="100"></div>
																			</div>
																		</div>
																	</div>
																	<div class="single-progress-bar">
																		<div class="progressbar-label">
																			<h5>{{ __('2 Star') }}<span class="f-right">{{ $two_rattings }}%</span></h5>
																			<div class="progress">
																				<div class="progress-bar w-{{ $two_rattings }}" role="progressbar" aria-valuenow="{{ $two_rattings }}" aria-valuemin="0" aria-valuemax="100"></div>
																			</div>
																		</div>
																	</div>
																	<div class="single-progress-bar">
																		<div class="progressbar-label">
																			<h5>{{ __('1 Star') }} <span class="f-right">{{ $one_rattings }}%</span></h5>
																			<div class="progress">
																				<div class="progress-bar w-{{ $one_rattings }}" role="progressbar" aria-valuenow="{{ $one_rattings }}" aria-valuemin="0" aria-valuemax="100"></div>
																			</div>
																		</div>
																	</div>
																</div>
															</div>
														</div>
													</div>
													<div class="review-all pt-30 pb-30">
														<div class="review-title pb-30">
															<h4>{{ __('All Reviews') }}</h4>
														</div>
														<div class="review-list">
															@foreach($store->vendor_reviews as $review)
															<div class="media">
																<img src="{{ asset(App\User::find($review->user_id)->avatar) }}" class="mr-3" alt="...">
																<div class="media-body">
																	<h5 class="mt-0 mb-10">{{ App\User::find($review->user_id)->name }} <span class="comment-date"> {{ $review->created_at->diffForHumans() }}</span>
																	</h5>
																	<div class="review-ratting">
																		@if($review->comment_meta->star_rate == 5)
																		<i class="fas fa-star"></i>
																		<i class="fas fa-star"></i>
																		<i class="fas fa-star"></i>
																		<i class="fas fa-star"></i>
																		<i class="fas fa-star"></i>
																		@endif
																		@if($review->comment_meta->star_rate == 4)
																		<i class="fas fa-star"></i>
																		<i class="fas fa-star"></i>
																		<i class="fas fa-star"></i>
																		<i class="fas fa-star"></i>
																		<i class="far fa-star"></i>
																		@endif
																		@if($review->comment_meta->star_rate == 3)
																		<i class="fas fa-star"></i>
																		<i class="fas fa-star"></i>
																		<i class="fas fa-star"></i>
																		<i class="far fa-star"></i>
																		<i class="far fa-star"></i>
																		@endif
																		@if($review->comment_meta->star_rate == 2)
																		<i class="fas fa-star"></i>
																		<i class="fas fa-star"></i>
																		<i class="far fa-star"></i>
																		<i class="far fa-star"></i>
																		<i class="far fa-star"></i>
																		@endif
																		@if($review->comment_meta->star_rate == 1)
																		<i class="fas fa-star"></i>
																		<i class="far fa-star"></i>
																		<i class="far fa-star"></i>
																		<i class="far fa-star"></i>
																		<i class="far fa-star"></i>
																		@endif
																	</div>
																	{{ $review->comment_meta->comment }}
																</div>
															</div>
															@endforeach
														</div>
													</div>
												</div>
											</div>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
				<div class="col-lg-3 p-0">
					<div class="store-right-section fixed">
						<div class="main_cart">
							<div class="delivery-main-content text-center">
								<form action="{{ route('checkout.index') }}" class="cartform">

								@if(Cart::instance('cart_'.$store->slug)->count() > 0)
								<div class="delivery-toogle-action">
									<span class="delivery-title">{{ __('Delivery') }}</span>
									<div class="custom-control custom-switch">
										<input type="checkbox" name="delivery_type" value="0" class="custom-control-input" id="uinque_id"> <label class="custom-control-label" for="uinque_id">{{ __('Pick Up') }}</label>
										</div>
								</div>
								<input type="hidden" id="pickup_price" value="{{ $store->pickup->content }}">
								<input type="hidden" id="delivery_price" value="{{ $store->delivery->content }}">
								<div class="delivery-avg-time" id="dummy">
									<i class="fas fa-truck"></i> {{ $store->delivery->content }} {{ __('min') }}
								</div>
								<div class="delivery-order-form">
									<h5>{{ __('Your order') }} {{ $store->name }}</h5>
								</div>
								<div class="cart-product-list">
									@foreach(Cart::instance('cart_'.$store->slug)->content() as $cart)
									<div class="single-cart-product d-flex">
										<div class="cart-product-title d-block">
											<h5>{{ $cart->name }}</h5>
											<p>{{ $cart->options->type }}</p>
										</div>
										<div class="cart-price-action d-block">
											<span>{{ strtoupper($currency->value) }} {{ number_format($cart->price,2) }}</span>
											<div class="cart-product-action d-flex">
												@if($cart->qty > 1)
												<a href="javascript:void(0)" class="right" onclick="limit_minus('{{ $cart->rowId }}','{{ $store->slug }}')"><span class="ti-minus"></span></a>
												@else
												<a href="javascript:void(0)" onclick="delete_cart('{{ $cart->rowId }}','{{ $store->slug }}')" class="right"><span class="fas fa-trash"></span></a>
												@endif
												<div class="qty">
													<input type="text" id="total_limit{{ $cart->rowId }}" value="{{ $cart->qty }}">
												</div>
												<a href="javascript:void(0)" class="left" onclick="limit_plus('{{ $cart->rowId }}','{{ $store->slug }}')"><span class="ti-plus"></span></a>
											</div>
										</div>
									</div>
									@endforeach
								</div>
								<div class="cart-product-another-information">
									<div class="single-information d-flex">
										<span>{{ __('Subtotal') }}</span>
										<div class="main-amount">
											<span>{{ __(strtoupper($currency->value)) }} {{ Cart::subtotal() }}</span>
										</div>
									</div>
									<div>
										<div class="checkout-btn">
											<a href="javascript:void(0)" onclick="$('.cartform').submit()">{{ __('Checkout') }}</a>
										</div>
									</div>
								</div>
								@else
								<h5 class="mt-20 mb-15">{{ __('No Item in your Cart') }}</h5>
								<p class="mb-15">{{ __("You haven't added anything in your cart yet! Start adding the products you like.") }}</p>
								<div class="cart-product-another-information">
									<div class="single-information d-flex">
										<span>{{ __('Subtotal') }}</span>
										<div class="main-amount">
											<span>{{ strtoupper($currency->value) }} {{ Cart::subtotal() }}</span>
										</div>
									</div>
									<div class="checkout-btn disabled">
										<a href="#" class="disabled">{{ __('Checkout') }}</a>
									</div>
								</div>
								@endif
							</form>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</section>
<!-- store area end -->

@if($store->status == 'offline')
<!-- Modal -->
<div class="modal fade" id="staticBackdrop" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="staticBackdropLabel" aria-hidden="true">
  <div class="modal-dialog modal-dialog-centered">
    <div class="modal-content">
      <div class="modal-body offline">
        <div class="close-resturants">
        	<div class="row">
        		<div class="col-lg-11">
        			<h3>{{ $store->name }} {{ __('is now closed') }}</h3>
        		</div>
        		<div class="col-lg-1">
        			<button type="button" class="close" data-dismiss="modal" aria-label="Close">
			          <span aria-hidden="true" class="ti-close"></span>
			        </button>
			    </div>
        	</div>
        	
        	<p>{{ __('The restaurant is closed right now. Check out others that are open or take a look at the menu to plan for your next meal.') }}</p>
        	<a href="{{ url('/') }}">{{ __('go to homepage') }}</a>
        	<a class="tranparent" href="#" data-dismiss="modal" aria-label="Close">{{ __('Close') }}</a>
        </div>
      </div>
    </div>
  </div>
</div>
@endif
<input type="hidden" id="store_url" value="{{ route('store_data',$store->slug) }}">
<input type="hidden" id="addon_url" value="{{ route('addon_product') }}">
@endsection

@push('js')

<script src="{{ theme_asset('khana/public/js/bootstrap-datetimepicker.min.js') }}"></script>
<script>
	"use strict";
    $(function () {
        $('#date').datetimepicker({
            format: "dd MM yyyy - HH:11 P",
            showMeridian: true,
            autoclose: true,
            todayBtn: true
        });
    })
</script>
@endpush