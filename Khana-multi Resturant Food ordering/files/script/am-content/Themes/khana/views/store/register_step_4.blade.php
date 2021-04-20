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
								<li class="active">
									<div class="register-progress-number">
										<span>2</span>
									</div>
									<div class="register-progress-body">
										{{ __('Step 2') }}
									</div>
								</li>
								<li class="active">
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
					<div class="register-main-card">
						<div class="row">
							<div class="col-lg-12">
								<div class="main-info text-center">
									<i class="far fa-check-circle"></i>
									<h4>{{ __("Your request is sent successfully and it's pending for approval") }}</h4>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</div>
@endsection