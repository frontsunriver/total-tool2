@section('style')

@endsection
@extends('layouts.backend.app')

@section('content')

<div class="card">
	<div class="card-body">
		<div class="card-action-filter">
			<div class="row mb-10">
				<div class="card-header">
					<div class="col-lg-10">
					<form id="basicform" method="post" action="{{ route('store.day.update') }}">
						@csrf
						<div class="d-flex">
							
							<div class="single-filter">
								<h4>{{ __('Shop Days') }}</h4>
							</div>
						</div>
					</div>
					<div class="col-lg-2">
						<div class="single-filter f-right">
							<div class="single-filter">
								<button type="submit" class="btn btn-primary btn-lg">{{ __('Update') }}</button>
							</div>
						</div>
					</div>
				</div>
			</div>
				</div>
			<div class="table-responsive custom-table">
				<table class="table">
					<thead>
						<tr>
							<th class="am-title">{{ __('Day') }}</th>
							<th class="am-tags">{{ __('Opening Time') }}</th>
							<th class="am-tags">{{ __('Closeing Time') }}</th>
							<th class="am-tags">{{ __('Status') }}</th>
						</tr>
					</thead>
					<tbody>

						@if(count($days) == 7)
						@foreach($days as $key => $row)
						<tr>
							<input type="hidden" name="day[]" value="{{ $row->day }}">
							<td>{{ $row->day }}</td>
							<td><input type="Time" name="opening[]" class="form-control" required="" value="{{ $row->opening }}"></td>
							<td><input type="Time" name="closeing[]" class="form-control" required=""  value="{{ $row->close }}"></td>
							<td>
							<select class="form-control"  name="status[]">
								<option value="1"@if($row->status==1) selected="" @endif>{{ __('Open') }}</option>
								<option value="0"@if($row->status==0) selected="" @endif>{{ __('Close') }}</option>
							</select>
						   </td>
							
						</tr>
						@endforeach

						@else
						<tr>
							<input type="hidden" name="day[]" value="saturday">

							<td>{{ __('Saturday') }}</td>
							<td><input type="Time" name="opening[]" class="form-control" required=""></td>
							<td><input type="Time" name="closeing[]" class="form-control" required=""></td>
							<td><select class="form-control" name="status[]">
								<option value="1">{{ __('Open') }}</option>
								<option value="0">{{ __('Close') }}</option>
							</select></td>
							
						</tr>
						<tr>
							<input type="hidden" name="day[]" value="sunday">
							<td>{{ __('Sunday') }}</td>
							<td><input type="Time" name="opening[]" class="form-control" required=""></td>
							<td><input type="Time" name="closeing[]" class="form-control" required=""></td>
							<td><select class="form-control" name="status[]">
								<option value="1">{{ __('Open') }}</option>
								<option value="0">{{ __('Close') }}</option>
							</select></td>
							
						</tr>
						<tr>
							<input type="hidden" name="day[]" value="monday">

							<td>{{ __('Monday') }}</td>
							<td><input type="Time" name="opening[]" class="form-control" required=""></td>
							<td><input type="Time" name="closeing[]" class="form-control" required=""></td>
							<td><select class="form-control" name="status[]">
								<option value="1">{{ __('Open') }}</option>
								<option value="0">{{ __('Close') }}</option>
							</select></td>
							
						</tr>
						<tr>
							<input type="hidden" name="day[]" value="tuesday">
							<td>{{ __('Tuesday') }}</td>
							<td><input type="Time" name="opening[]" class="form-control" required=""></td>
							<td><input type="Time" name="closeing[]" class="form-control" required=""></td>
							<td><select class="form-control" name="status[]">
								<option value="1">{{ __('Open') }}</option>
								<option value="0">{{ __('Close') }}</option>
							</select></td>
							
						</tr>
						<tr>
							<input type="hidden" name="day[]" value="wednesday">

							<td>{{ __('Wednesday') }}</td>
							<td><input type="Time" name="opening[]" class="form-control" required=""></td>
							<td><input type="Time" name="closeing[]" class="form-control" required=""></td>
							<td><select class="form-control" name="status[]">
								<option value="1">{{ __('Open') }}</option>
								<option value="0">{{ __('Close') }}</option>
							</select></td>
							
						</tr>
						<tr>
							<input type="hidden" name="day[]" value="thursday">

							<td>{{ __('Thursday') }}</td>
							<td><input type="Time" name="opening[]" class="form-control" required=""></td>
							<td><input type="Time" name="closeing[]" class="form-control" required=""></td>
							<td><select class="form-control" name="status[]">
								<option value="1">{{ __('Open') }}</option>
								<option value="0">{{ __('Close') }}</option>
							</select></td>
							
						</tr>
						<tr>
							<input type="hidden" name="day[]" value="friday">

							<td>{{ __('Friday') }}</td>
							<td><input type="Time" name="opening[]" class="form-control" required=""></td>
							<td><input type="Time" name="closeing[]" class="form-control" required=""></td>
							<td><select class="form-control" name="status[]">
								<option value="1">{{ __('Open') }}</option>
								<option value="0">{{ __('Close') }}</option>
							</select></td>
							
						</tr>
						@endif
					</tbody>

					<tfoot>
						<tr>
							<th class="am-title">{{ __('Day') }}</th>
							<th class="am-tags">{{ __('Opening Time') }}</th>
							<th class="am-tags">{{ __('Closeing Time') }}</th>
							<th class="am-tags">{{ __('Status') }}</th>
						</tr>
					</tfoot>
				</table>
		</div>
	</div>
</div>
@endsection

@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
@endsection