<div class="single-order-card">
	<div class="row align-items-center">
		@php 
		$client_data = json_decode($order->data);
		@endphp
		<div class="col">
			<div class="time-date">
				<span>{{ $order->updated_at->diffForHumans() }}</span>
			</div>
			<div class="row align-items-center">
				<div class="col-lg-8">
					<div class="status"></div>
					<div class="order-name">
						@php 
						$user = App\User::find($order->vendor_id);
						@endphp
						<a href="{{ route('rider.order.details',$order->id) }}">#{{ $order->id }} {{ $user->name }}</a>
					</div>
				</div>
				<div class="col">
					<a href="{{ route('rider.order.details',$order->id) }}" class="btn btn-primary ml-auto">{{ __('Details') }}</a>
				</div>
			</div>
			<div class="client-name">
				<span>{{ $client_data->name }}</span>
			</div>
		</div>
	</div>
</div>