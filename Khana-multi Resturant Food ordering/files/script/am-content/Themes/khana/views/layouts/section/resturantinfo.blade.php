@php 
$json_info = json_decode($store->info->content);
@endphp
<div class="single-category-food restaurantsinfo_open mt-50 mb-50">
	<div class="single-category-main-content">
		<div class="row">
			<div class="col-lg-6">
				<div class="restaurant-info-left-section">
					<h3 class="mb-20">{{ __('Restaurant Info') }}</h3>
					<p class="mb-20">{{ $json_info->description }}</p>
					<p class="mb-20">{{ $json_info->full_address }}</p>
					<div class="contact-info">
						<div class="single-content">
							<i class="fas fa-phone"></i> {{ $json_info->phone1 }}, {{ $json_info->phone2 }}
						</div>
						<div class="contact-info">
							<div class="single-content">
								<i class="far fa-envelope"></i> {{ $json_info->email1 }}, {{ $json_info->email2 }}
							</div>
						</div><br>
						<div class="contact-info">
							@foreach($store->shopday as $day)
							<div class="single-content">
								 @if($day->status==1)
								<i class="far fa-clock"></i> {{ strtoupper($day->day) }}
								 								
								 {{ $day->opening }} - {{ $day->close }} 
								
								@endif
							</div>
							@endforeach
						</div>
					</div>
				</div>
			</div>
			<div class="col-lg-6">
				<div class="restaurant-iframe-map">
					<iframe class="map-size b-0" src="https://maps.google.com/maps?q={{ $store->location->latitude }},{{ $store->location->longitude }}&amp;output=embed"  allowfullscreen=""></iframe><br />
				</div>
			</div>
		</div>
		<hr>
		<div class="row">
			<div class="col-lg-12">
				<div class="restaurant-info-right-section">
					<h3 class="mb-15">{{ __('Cuisine') }}</h3>
					<div class="restaurant-service-list">
						<nav>
							<ul>
								@foreach($store->shopcategory as $category)
								<li><a href="#"><i class="far fa-check-circle"></i>
									{{ $category->name }}</a></li>
									@endforeach
								</ul>
							</nav>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>