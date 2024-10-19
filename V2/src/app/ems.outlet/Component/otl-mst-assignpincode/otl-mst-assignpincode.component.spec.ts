import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlMstAssignpincodeComponent } from './otl-mst-assignpincode.component';

describe('OtlMstAssignpincodeComponent', () => {
  let component: OtlMstAssignpincodeComponent;
  let fixture: ComponentFixture<OtlMstAssignpincodeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlMstAssignpincodeComponent]
    });
    fixture = TestBed.createComponent(OtlMstAssignpincodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
