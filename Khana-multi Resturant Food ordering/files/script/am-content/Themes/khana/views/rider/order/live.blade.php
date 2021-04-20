@extends('layouts.backend.app')
@section('content')
	<div class="row">
				<div class="col-lg-4">
					<div class="card">
						<div class="card-header">
							<h4>{{ __('New Orders') }}</h4>
						</div>
						<div class="card-body" id="neworders">
							
						</div>
					</div>
				</div>
				<div class="col-lg-4">
          <div class="card">
            <div class="card-header">
              <h4>{{ __('Accepted') }}</h4>
            </div>
            <div class="card-body">
              @foreach($accepteorders as $order)
              @include('theme::rider.section.singleorder')
              @endforeach
            </div>
          </div>
        </div>
			<div class="col-lg-4">
          <div class="card">
            <div class="card-header">
              <h4>{{ __('Done') }}</h4>
            </div>
            <div class="card-body">
              @foreach($completeorders as $order)
              @include('theme::rider.section.singleorder')
              @endforeach
            </div>
        </div>
    </div>
</div>
<input type="hidden" value="" id="ringurl">		
@php
$currency=\App\Options::where('key','currency_name')->select('value')->first();
@endphp	
@endsection

@section('script')
<script type="text/javascript">
	"use strict";
	var baseurl= "{{ route('store.order.create') }}";
	var mainUrl= "{{ url('/') }}";
	var currency= "{{ strtoupper($currency->value) }}";
  var ringurl = '{{ url('uploads/audio/ring.mp3') }}';
</script>
<script src="{{ theme_asset('khana/public/js/rider/live.js') }}"></script>
@endsection