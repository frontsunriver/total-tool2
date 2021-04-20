@extends('theme::layouts.app')

@section('content')
<div class="main-content mt-50">
	<div class="container">
		<div class="row">
			<div class="col-lg-6 offset-lg-3">
				<div class="login-card">
					<div class="login-header">
						<h5>{{ __('Don’t have an Account? Register now!') }}</h5>
					</div>	
					<div class="login-body">
						<div class="login-form">
							@if(Session::has('errors'))
							<div class="row">
								<div class="col-12">
									<p class="alert alert-danger">{{ Session::get('errors') }}</p>
								</div>
							</div>
							@endif
							<form method="POST" action="{{ route('user.register') }}">
								@csrf
								<div class="form-group">
									<label>{{ __('Your Full Name') }}</label>
									<input type="text" name="name" class="form-control">
								</div>
								<div class="form-group">
									<label>{{ __('Your Email') }}</label>
									<input type="email" name="email" class="form-control">
								</div>
								<div class="form-group">
									<label>{{ __('Password') }}</label>
									<input type="password" class="form-control" name="password">
								</div>
								<div class="form-group">
									<label>{{ __('Password') }}</label>
									<input type="password" class="form-control" name="password_confirmation">
								</div>
								<div class="remember-section">
									<div class="remember">
										<div class="custom-control custom-checkbox">
											<input type="checkbox" class="custom-control-input area" id="agree" name="agree">
											<label class="custom-control-label" for="agree">{{ __('I agree to') }} <a href="{{ url('/page/terms-and-conditions') }}">{{ __('Terms & Conditions') }}</a></label>
										</div>
									</div>
								</div>
								<div class="login-button">
									<button type="submit">{{ __('Register Now') }}</button>
								</div>
							</form>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</div>
@endsection