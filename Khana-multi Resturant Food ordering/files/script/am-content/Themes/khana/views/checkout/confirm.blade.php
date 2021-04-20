@extends('theme::layouts.app')

@section('content')
<div class="confirm-area">
	<div class="container">
		<div class="row mt-50">
			<div class="col-lg-6 offset-lg-3">
				<div class="confirmation-page text-center">
					<i class="fas fa-check-circle"></i>
					<div class="order-confirm">
						<h4>{{ __('Your Order is Confirmed') }}</h4>
						<a href="{{ route('author.dashboard') }}">{{ __('View Order') }}</a>
					</div>
				</div>
			</div>
		</div>
	</div>
</div>
@endsection