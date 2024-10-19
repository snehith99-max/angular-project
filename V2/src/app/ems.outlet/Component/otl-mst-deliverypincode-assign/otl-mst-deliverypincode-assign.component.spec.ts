import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlMstDeliverypincodeAssignComponent } from './otl-mst-deliverypincode-assign.component';

describe('OtlMstDeliverypincodeAssignComponent', () => {
  let component: OtlMstDeliverypincodeAssignComponent;
  let fixture: ComponentFixture<OtlMstDeliverypincodeAssignComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlMstDeliverypincodeAssignComponent]
    });
    fixture = TestBed.createComponent(OtlMstDeliverypincodeAssignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
