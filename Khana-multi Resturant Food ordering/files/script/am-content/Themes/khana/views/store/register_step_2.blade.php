@extends('theme::layouts.app')
@push('css')
<script async defer src="https://maps.googleapis.com/maps/api/js?key={{ env('PLACE_KEY') }}&libraries=places&callback=initialize"></script>
@endpush
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

					<form action="{{ route('restaurant.register_step_2') }}" method="POST">
						@csrf
						<div class="register-card-body">
							<div class="row mt-30">
								@if(Session::has('errors'))
								<div class="col-lg-12">
									<p class="alert alert-danger">{{ Session::get('errors') }}</p>
								</div>
								@endif
								<div class="col-lg-12">
									<div class="form-group">
										<label>{{ __('Select City') }}</label>
										<select name="city" id="cty" class="form-control selectric">
											@foreach($cities as $city)
											<option value="{{ $city->id }}">{{ $city->title }}</option>
											@endforeach
										</select>
									</div>
								</div>
								<div class="col-lg-12">
									<div class="form-group">
										<label>{{ __('Address Line') }}</label>
										<input  type="text" class="form-control" required="" name="address_line">
									</div>
								</div>
								<div class="col-lg-12">
									<div class="form-group">
										<label>{{ __('Select Your Location') }}</label>
										<input  type="text" class="form-control" required="" name="full_address" id="location_input" required>
									</div>
								</div>
								<input type="hidden" name="latitude" id="latitude" value="00.00">
								<input type="hidden" name="longitude" id="longitude" value="00.00">
								<div class="col-lg-12" id="map-area">
									<label>{{ __('Drag Your Location') }}</label>
									<div id="map-canvas" class="map-canvas h-300"></div>
								</div>
								<div class="col-lg-12">
									<div class="f-right">
										<button class="btn btn-danger">{{ __('Next & Save') }}</button>
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

@push('js')

<script src="{{ theme_asset('khana/public/js/frontend/storeregister.js') }}"></script>

@endpush