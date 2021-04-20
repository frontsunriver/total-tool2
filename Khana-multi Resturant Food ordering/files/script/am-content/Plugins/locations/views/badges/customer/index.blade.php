@extends('layouts.backend.app')

@section('content')
<div class="card"  >
	<div class="card-body">
		<div class="row mb-30">
			<div class="col-lg-6">
				<h4>{{ __('Customer Badge List') }}</h4>
			</div>
			<div class="col-lg-6">
				<div class="add-new-btn">
					<a href="{{ route('admin.badge.create','type='.$type) }}" class="btn f-right btn-primary ">{{ __('Add New') }}</a>
				</div>
			</div>
		</div>
	
		<div class="cart-filter mb-20">
			<a href="{{ route('admin.badge.index','type=customer') }}">{{ __('All') }} <span>({{ App\Terms::where('type',5)->where('status','!=',0)->count() }})</span></a>
			<a href="?st=1&type=customer">{{ __('Published') }} <span>({{ App\Terms::where('type',5)->where('status',1)->count() }})</span></a>
			<a href="?st=2&type=customer">{{ __('Drafts') }} <span>({{ App\Terms::where('type',5)->where('status',2)->count() }})</span></a>
			<a href="?st=trash&type=customer" class="trash">{{ __('Trash') }} <span>({{ App\Terms::where('type',5)->where('status',0)->count() }})</span></a>
		</div>
		<div class="card-action-filter">
			<form method="post" id="basicform" action="{{ route('admin.badges.destroy') }}">
				@csrf
				<div class="row">
					<div class="col-lg-6">
						<div class="d-flex">
							<div class="single-filter">
								<div class="form-group">
									<select class="form-control" name="status">
										<option value="publish">{{ __('Publish') }}</option>
										<option value="trash">{{ __('Move to Trash') }}</option>
										<option value="delete">{{ __('Delete Permanently') }}</option>

									</select>
								</div>
							</div>
							<div class="single-filter">
								<button type="submit" class="btn btn-primary ml-2 mt-1">{{ __('Apply') }}</button>
							</div>
						</div>
					</div>
					<div class="col-lg-6">
						<div class="single-filter f-right">
							<div class="form-group">
								<input type="text" id="data_search" class="form-control" placeholder="Enter Value">
							</div>
						</div>
					</div>
				</div>
			</div>
			<div class="table-responsive custom-table">
				<table class="table">
					<thead>
						<tr>
							<th class="am-select">
								<div class="custom-control custom-checkbox">
									<input type="checkbox" class="custom-control-input checkAll" id="customCheck12">
									<label class="custom-control-label checkAll" for="customCheck12"></label>
								</div>
							</th>
							<th class="am-title"><i class="far fa-image"></i></th>
							<th class="am-title">{{ __('Title') }}</th>
							<th class="am-title">{{ __('Amount') }}</th>
							<th class="am-title">{{ __('Is Default') }}</th>
							<th class="am-title">{{ __('Total Users') }}</th>
							<th class="am-title">{{ __('Status') }}</th>
							<th class="am-date">{{ __('Date') }}</th>
						</tr>
					</thead>
					<tbody>
						@foreach($posts as $post)
						<tr>
							<th>
								@if($post->count == 0)
								<div class="custom-control custom-checkbox">
									<input type="checkbox" name="ids[]" class="custom-control-input" id="customCheck{{ $post->id }}" value="{{ $post->id }}">
									<label class="custom-control-label" for="customCheck{{ $post->id }}"></label>
								</div>
								@endif
							</th>
							<td>
								<img src="{{ asset($post->preview->content) }}" height="50">
							</td>
							<td>
								{{ $post->title }}
								<div class="hover">
									<a href="{{ route('admin.badge.edit',$post->id) }}">{{ __('Edit') }}</a>

									<a href="{{ route('admin.badge.show',$post->id) }}" class="last">{{ __('View') }}</a>
								</div>
							</td>
							<td>{{ $post->slug }}</td>
							@if($post->count == 1)
							<td>						
							<span class="badge badge-success">{{ __('Yes') }}</span>
							</td>
							@else
							<td><span class="badge badge-danger">{{ __('No') }}</span></td>
							@endif
							<td>{{ $post->badgeusers_count }}</td>							
							<td>@if($post->status==1)  {{ __('Published') }} @elseif($post->status==2)  Draft @else {{ __('Trash') }} @endif</td>
							
							
							<td>{{ __('Last Modified') }}
								<div class="date">
									{{ $post->updated_at->diffForHumans() }}
								</div>
							</td>
						</tr>
						@endforeach
					</tbody>
				</form>
				<tfoot>
					<tr>
						<th class="am-select">
							<div class="custom-control custom-checkbox">
								<input type="checkbox" class="custom-control-input checkAll" id="customCheck12">
								<label class="custom-control-label checkAll" for="customCheck12"></label>
							</div>
						</th>
						<th class="am-title"><i class="far fa-image"></i></th>
						<th class="am-title">{{ __('Title') }}</th>
						<th class="am-title">{{ __('Amount') }}</th>
						<th class="am-title">{{ __('Is Default') }}</th>
						<th class="am-title">{{ __('Total Users') }}</th>			
						<th class="am-title">{{ __('Status') }}</th>			
						<th class="am-date">{{ __('Date') }}</th>
					</tr>
				</tfoot>
			</table>
			{{ $posts->links() }}

		</div>
	</div>
</div>
@endsection
@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script type="text/javascript">
	"use strict";	
	//response will assign this function
	function success(res){
		location.reload();
	}
</script>
@endsection