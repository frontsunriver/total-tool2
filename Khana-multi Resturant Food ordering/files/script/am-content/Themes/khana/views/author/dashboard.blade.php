@extends('theme::layouts.app')

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
<!-- main area start -->
<section>
	<div class="main-area pt-50">
		<div class="container">
			<div class="row">
				<div class="col-lg-4">
					<div class="settings-sidebar-card">
						<div class="profile-show-area text-center">
							<div class="profile-img">
								<img src="{{ asset(Auth::User()->avatar) }}" alt="">
							</div>
							<div class="profile-content">
								<h5>{{ Auth::User()->name }}</h5>
								<span>{{ Auth::User()->email }}</span>
							</div>
						</div>
						<div class="settings-main-menu">
							<nav>
								<ul class="nav nav-tabs">
									<li>
										<a href="#dashbaord" data-toggle="tab" class="active">
											<i class="fas fa-tachometer-alt"></i> {{ __('Dashboard') }}
										</a>
									</li>
									<li>
										<a href="#orders" data-toggle="tab">
											<i class="fas fa-clone"></i> {{ __('Orders') }}
										</a>
									</li>
									<li>
										<a href="#settings" data-toggle="tab">
											<i class="fas fa-cog"></i> {{ __('Settings') }}
										</a>
									</li>
									<li>
										<a href="#ratting_menu" data-toggle="tab">
											<i class="fas fa-star"></i> {{ __('Rattings & Reviews') }}
										</a>
									</li>
									<li>
										<a href="{{ route('logout') }}" onclick="event.preventDefault();document.getElementById('logout-form').submit();" data-toggle="tab">
											<i class="fas fa-sign-out-alt"></i> {{ __('Logout') }}
										</a>
										<form id="logout-form" action="{{ route('logout') }}" method="POST" class="d-none">
								        @csrf
								      </form>
									</li>
								</ul>
							</nav>
						</div>
					</div>
				</div>
				<div class="col-lg-8">
					<div class="tab-content">
						<div class="setting-main-area tab-pane fade in active show" id="dashbaord">
							<div class="settings-content-area">
								<div class="row">
									@if (\Session::has('error'))
									<div class="col-lg-12">
										<div class="alert alert-danger">
											<ul>
												<li>{!! \Session::get('error') !!}</li>
											</ul>
										</div>
									</div> 
									@endif
									<div class="col-lg-6">
										<div class="single-dashboard-widget d-flex">
											<div class="left-icon">
												<i class="fas fa-clone"></i>
											</div>
											<div class="right-area f-right">
												<h5>{{ __('Total Orders') }}</h5>
												<span>{{ App\Order::where('user_id',Auth::User()->id)->count() }}</span>
											</div>
										</div>
									</div>
									<div class="col-lg-6">
										<div class="single-dashboard-widget d-flex">
											<div class="left-icon">
												<i class="fab fa-first-order-alt"></i>
											</div>
											<div class="right-area f-right">
												<h5>{{ __('Pending Orders') }}</h5>
												<span>{{ App\Order::where([
													['user_id',Auth::User()->id],
													['status',2]
													])->count() }}</span>
												</div>
											</div>
										</div>
									</div>
									@php
									$orders=\App\Order::where('user_id',Auth::User()->id)->orderBy('id','DESC')->paginate(20)
									@endphp
									<div class="row mt-30">
										<div class="col-lg-12">
											<div class="table-responsive">
												<table class="table">
													<thead class="thead-dark">
														<tr>
															<th scope="col">{{ __('Order Id') }}</th>
															<th scope="col">{{ __('Payment Method') }}</th>
															<th scope="col">{{ __('Status') }}</th>
															<th scope="col">{{ __('Amount') }}</th>
															<th scope="col">{{ __('Action') }}</th>
														</tr>
													</thead>
													<tbody>
														@foreach($orders as $order)
														<tr>
															<th>{{ $order->id }}</th>
															<td>{{ $order->payment_method }}</td>
															<td>
																@if($order->status == 2)
																<div class="badge badge-primary">{{ __('pending') }}</div>
																@elseif($order->status == 3)
																<div class="badge badge-info">{{ __('pickup') }}</div>
																@elseif($order->status == 1)
																<div class="badge badge-info">{{ __('complete') }}</div>
																@elseif($order->status == 0)
																<div class="badge badge-danger">{{ __('cancel') }}</div>
																@endif
															</td>
															<td>{{ strtoupper($currency->value) }} {{ $order->total + $order->shipping }}</td>
															<td>
																<div class="order-btn d-flex">
																	@if($order->status == 1)
																	@if(!$order->review()->count() > 0)
																	<a class="view_btn mr-2 btn-send" href="#" data-toggle="modal" data-target="#send_review_{{ $order->id }}"><i class="fas fa-paper-plane"></i></i></a>
																	@endif
																	@endif
																	<a class="view_btn" href="{{ route('author.order.details',encrypt($order->id)) }}"><i class="fas fa-eye"></i></a>
																</div>
															</td>
														</tr>


														@if($order->status == 1)
														@if(!$order->review()->count() > 0)
														<div class="modal fade" id="send_review_{{ $order->id }}" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
															<div class="modal-dialog">
																<div class="modal-content">
																	<div class="modal-header">
																		<h5 class="modal-title" id="exampleModalLabel">{{ __('Send Review') }}</h5>
																		<button type="button" class="close" data-dismiss="modal" aria-label="Close">
																			<span aria-hidden="true">&times;</span>
																		</button>
																	</div>
																	<form action="{{ route('author.review') }}" method="POST">
																		@csrf
																		<div class="modal-body">
																			<div class="form-group">
																				<label for="ratting" class="col-form-label">{{ __('Select Ratting') }}:</label>
																				<select id="ratting" class="form-control" name="ratting">
																					<option value="5">{{ __('5 Star') }}</option>
																					<option value="4">{{ __('4 Star') }}</option>
																					<option value="3">{{ __('3 Star') }}</option>
																					<option value="2">{{ __('2 Star') }}</option>
																					<option value="1">{{ __('1 Star') }}</option>
																				</select>
																			</div>
																			<input type="hidden" name="vendor_id" value="{{ $order->vendor_id }}">
																			<input type="hidden" name="order_id" value="{{ $order->id }}">
																			<div class="form-group">
																				<label for="review" class="col-form-label">{{ __('Write Review') }}:</label>
																				<textarea class="form-control" id="review" name="review"></textarea>
																			</div>
																		</div>
																		<div class="modal-footer">
																			<button type="button" class="btn btn-secondary" data-dismiss="modal">{{ __('Close') }}</button>
																			<button type="submit" class="btn btn-primary">{{ __('Send Review') }}</button>
																		</div>
																	</form>
																</div>
															</div>
														</div>
														@endif
														@endif
														@endforeach
													</tbody>
												</table>
											</div>

											{{ $orders->links() }}
										</div>
									</div>
								</div>
							</div>
							<div class="setting-main-area verification_area tab-pane fade" id="orders">
								<div class="settings-content-area">
									<h4>Orders</h4>
									<div class="row">
										<div class="col-lg-12">
											<div class="table-responsive">
												<table class="table">
													<thead class="thead-dark">
														<tr>
															<th scope="col">{{ __('Order Id') }}</th>
															<th scope="col">{{ __('Payment Method') }}</th>
															<th scope="col">{{ __('Status') }}</th>
															<th scope="col">{{ __('Amount') }}</th>
															<th scope="col">{{ __('Action') }}</th>
														</tr>
													</thead>
													<tbody>
														@foreach($orders as $order)
														<tr>
															<th>{{ $order->id }}</th>
															<td>{{ $order->payment_method }}</td>
															<td>
																@if($order->status == 2)
																<div class="badge badge-primary">{{ __('pending') }}</div>
																@elseif($order->status == 3)
																<div class="badge badge-info">{{ __(
																'pickup') }}</div>
																@elseif($order->status == 1)
																<div class="badge badge-info">{{ __('complete') }}</div>
																@elseif($order->status == 0)
																<div class="badge badge-danger">{{ 
																__('cancel') }}</div>
																@endif
															</td>
															<td>{{ strtoupper($currency->value) }}{{ $order->total + $order->shipping }}</td>
															<td>
																<div class="order-btn d-flex">
																	@if($order->status == 1)
																	@if(!$order->review()->count() > 0)
																	<a class="view_btn mr-2 btn-send" href="#" data-toggle="modal" data-target="#send_review{{ $order->id }}"><i class="fas fa-paper-plane"></i></i></a>
																	@endif
																	@endif
																	<a class="view_btn" href="{{ route('author.order.details',encrypt($order->id)) }}"><i class="fas fa-eye"></i></a>
																</div>
															</td>
														</tr>
														@if($order->status == 1)
														@if(!$order->review()->count() > 0)
														<div class="modal fade" id="send_review{{ $order->id }}" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
															<div class="modal-dialog">
																<div class="modal-content">
																	<div class="modal-header">
																		<h5 class="modal-title" id="exampleModalLabel">{{ __('Send Review') }}</h5>
																		<button type="button" class="close" data-dismiss="modal" aria-label="Close">
																			<span aria-hidden="true">&times;</span>
																		</button>
																	</div>
																	<form action="{{ route('author.review') }}" method="POST">
																		@csrf
																		<div class="modal-body">
																			<div class="form-group">
																				<label for="ratting" class="col-form-label">{{ __('Select Ratting') }}:</label>
																				<select id="ratting" class="form-control" name="ratting">
																					<option value="5">{{ __('5 Star') }}</option>
																					<option value="4">{{ __('4 Star') }}</option>
																					<option value="3">{{ __('3 Star') }}</option>
																					<option value="2">{{ __('2 Star') }}</option>
																					<option value="1">{{ __('1 Star') }}</option>
																				</select>
																			</div>
																			<input type="hidden" name="vendor_id" value="{{ $order->vendor_id }}">
																			<input type="hidden" name="order_id" value="{{ $order->id }}">
																			<div class="form-group">
																				<label for="review" class="col-form-label">{{ __('Write Review') }}:</label>
																				<textarea class="form-control" id="review" name="review"></textarea>
																			</div>
																		</div>
																		<div class="modal-footer">
																			<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
																			<button type="submit" class="btn btn-primary">{{ __('Send Review') }}</button>
																		</div>
																	</form>
																</div>
															</div>
														</div>
														@endif
														@endif
														@endforeach
													</tbody>
												</table>
											</div>
											{{ $orders->links() }}
										</div>
									</div>
								</div>
							</div>
							<div class="setting-main-area verification_area tab-pane fade" id="settings">
								<div class="settings-content-area">
									<h4>{{ __('Settings') }}</h4>
									<form action="{{ route('author.settings.update') }}" method="POST" id="user_settings_form">
										@csrf
										<div class="row">
											<div class="col-lg-6">
												<div class="form-group">
													<label for="name">{{ __('Name') }}</label>
													<input type="text" class="form-control" name="name" placeholder="{{ __('Name') }}" id="name" value="{{ Auth::User()->name }}">
												</div>
											</div>
											<div class="col-lg-6">
												<div class="form-group">
													<label for="email">{{ __('Email') }}</label>
													<input type="text" class="form-control" name="email" placeholder="{{ __('Email') }}" id="email" value="{{ Auth::User()->email }}">
												</div>
											</div>
											<div class="col-lg-12">
												<h5>{{ __('Password Change') }}</h5>
											</div>
											<div class="col-lg-12">
												<div class="form-group">
													<label for="current_password">{{ __('Current Password') }}</label>
													<input type="password" class="form-control" placeholder="{{ __('Current Password') }}" name="current_password" id="current_password">
												</div>
											</div>
											<div class="col-lg-6">
												<div class="form-group">
													<label for="new_password">{{ __('New Password') }}</label>
													<input type="password" class="form-control" placeholder="{{ __('New Password') }}" name="password" id="new_password">
												</div>
											</div>
											<div class="col-lg-6">
												<div class="form-group">
													<label for="confirm_password">{{ __('Confirm Password') }}</label>
													<input type="password" class="form-control" placeholder="Confirm Password" name="password_confirmation" id="confirm_password">
												</div>
											</div>
											<div class="col-lg-12">
												<div class="btn-submit f-right">
													<button type="submit">{{ __('Update') }}</button>
												</div>
											</div>
										</div>
									</form>
								</div>
							</div>
							<div class="setting-main-area verification_area tab-pane fade" id="ratting_menu">
								<div class="settings-content-area">
									<h4>{{ __('Rattings & Reviews') }}</h4>
									<div class="row">
										<div class="col-lg-12">
											<div class="table-responsive">
												<table class="table">
													<thead class="thead-dark">
														<tr>
															<th scope="col">#</th>
															<th scope="col">{{ __('Resturant Name') }}</th>
															<th scope="col">{{ __('Ratting') }}</th>
															<th scope="col">{{ __('Review') }}</th>
															<th scope="col">{{ __('Action') }}</th>
														</tr>
													</thead>
													<tbody>
														@foreach(Auth::User()->user_reviews as $key=>$review)
														<tr>
															<th>{{ $key + 1 }}</th>
															<td><a target="__blank" href="{{ url('store',App\User::find($review->vendor_id)->slug) }}">{{ App\User::find($review->vendor_id)->name }}</a></td>
															<td>{{ $review->comment_meta->star_rate }} {{ __('Star') }}</td>
															<td>{{ Str::limit($review->comment_meta->comment,20) }}</td>
															<td><a class="view_btn" href="#"><i class="fas fa-eye"></i></a></td>
														</tr>
														@endforeach
													</tbody>
												</table>
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
	</section>
	<!-- main area end -->
	@endsection