@extends('theme::layouts.app')

@section('content')
<div class="main-content mt-50">
	<div class="container">
		<div class="row">
			<div class="col-lg-10 offset-lg-1">
				<div class="register-card">
					<div class="register-progress text-center">
						<nav>
							<ul>
								<li class="active">
									<div class="register-progress-number">
										<span>1</span>
									</div>
									<div class="register-progress-body">
										{{ __('Step 1') }}
									</div>
								</li>
								<li>
									<div class="register-progress-number">
										<span>2</span>
									</div>
									<div class="register-progress-body">
										{{ __('Step 2') }}
									</div>
								</li>
								<li>
									<div class="register-progress-number">
										<span>3</span>
									</div>
									<div class="register-progress-body">
										{{ __('Step 3') }}
									</div>
								</li>
								
							</ul>
						</nav>
					</div>
					<form action="{{ route('restaurant.register') }}" method="POST">
						@csrf
						<div class="register-card-body">
							<div class="row mt-30">
								@if(Session::has('errors'))
								<div class="col-lg-12">
									<p class="alert alert-danger">{{ Session::get('errors') }}</p>
								</div>
								@endif
								<div class="col-lg-6">
									<div class="form-group">
										<label>{{ __('Restaurant Name') }}</label>

										<input  type="text" required="" name="name" class="form-control">
									</div>
								</div>
								<div class="col-lg-6">
									<div class="form-group">
										<label>{{ __('Email') }}</label>
										<input type="email" required="" name="email" class="form-control" >
									</div>
								</div>
								<div class="col-lg-6">
									<div class="form-group">
										<label>{{ __('Password') }}</label>
										<input  type="password" class="form-control" required="" name="password">
									</div>
								</div>
								<div class="col-lg-6">
									<div class="form-group">
										<label>{{ __('Confirm Password') }}</label>
										<input  type="password" class="form-control" required="" name="password_confirmation">
									</div>
								</div>
								<div class="col-lg-6">
									<div class="form-group">
										<label>{{ __('Delivery Avg Time(Min)') }}</label>
										<input  type="number" class="form-control" required="" name="delivery">
									</div>
								</div>
								<div class="col-lg-6">
									<div class="form-group">
										<label>{{ __('Pick Up Avg Time(Min)') }}</label>
										<input  type="number" class="form-control" required="" name="pickup">
									</div>
								</div>
								<div class="col-lg-6">
									<div class="form-group">
										<label>{{ __('Support Phone Number 1') }}</label>
										<input  type="number" class="form-control" required="" name="phone_number_1">
									</div>
								</div>
								<div class="col-lg-6">
									<div class="form-group">
										<label>{{ __('Support Phone Number 2') }}</label>
										<input  type="number" class="form-control" required="" name="phone_number_2">
									</div>
								</div>
								<div class="col-lg-6">
									<div class="form-group">
										<label>{{ __('Support Email Address 1') }}</label>
										<input  type="email" class="form-control" required="" name="email_address_1">
									</div>
								</div>
								<div class="col-lg-6">
									<div class="form-group">
										<label>{{ __('Support Email Address 2') }}</label>
										<input  type="email" class="form-control" required="" name="email_address_2">
									</div>
								</div>
								<div class="col-lg-12">
									<div class="form-group">
										<label>{{ __('Description') }}</label>
										<textarea  rows="4" class="form-control" required="" name="description"></textarea>
									</div>
								</div>
								<div class="col-lg-12">
									<div class="f-right">
										<button>{{ __('Next & Save') }}</button>
									</div>
								</div>
							</div>
						</div>
					</form>	
				</div>
			</div>
		</div>
	</div>
</div>
@endsection