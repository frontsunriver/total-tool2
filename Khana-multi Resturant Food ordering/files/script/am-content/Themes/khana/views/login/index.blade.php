@extends('theme::layouts.app')

@section('content')
<div class="main-content mt-50">
	<div class="container">
		<div class="row">
			<div class="col-lg-6 offset-lg-3">
				<div class="login-card">
					<div class="login-header">
						<h5>{{ __('Login your account') }}</h5>
					</div>	
					<div class="login-body">
						<div class="social-login">
							<h6>{{ __('Login with social account') }}</h6>
							<div class="social-links">
								@if(env('FACEBOOK_CLIENT_ID') != null)
								<a class="facebook" href="{{ url('login/facebook') }}"><i class="fab fa-facebook"></i> {{ __('Facebook') }}</a>
								@endif

								@if(env('GOOGLE_CLIENT_ID') != null)
								<a class="google" href="{{ url('login/google') }}"><i class="fab fa-google"></i> {{ __('Google') }}</a>
								@endif
							</div>
						</div>
						<div class="login-form">
							<form action="{{ route('login') }}" method="POST">
								@csrf
								<div class="form-group">
									<label>{{ __('Email') }}</label>
									<input type="email" name="email" class="form-control @error('email') is-invalid @enderror" name="email" value="{{ old('email') }}" required autocomplete="email" autofocus>
									@error('email')
									<span class="invalid-feedback" role="alert">
										<strong>{{ $message }}</strong>
									</span>
									@enderror
								</div>
								<div class="form-group">
									<label>{{ __('Password') }}</label>
									<input type="password" class="form-control @error('password') is-invalid @enderror" name="password" required autocomplete="current-password">
									@error('password')
									<span class="invalid-feedback" role="alert">
										<strong>{{ $message }}</strong>
									</span>
									@enderror
								</div>
								<div class="remember-section d-flex">
									<div class="remember">
										<div class="custom-control custom-checkbox">
											<input type="checkbox" class="custom-control-input area" id="remember" name="remember" {{ old('remember') ? 'checked' : '' }}>
											<label class="custom-control-label" for="remember">{{ __('Remember Me') }}</label>
										</div>
									</div>
									<div class="forgotten">
										@if(Route::has('password.request'))
										<a href="{{ route('password.request') }}">{{ __('Forgot password?') }}</a>
										@endif
									</div>
								</div>
								<div class="login-button">
									<button type="submit">{{ __('Login Now') }}</button>
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