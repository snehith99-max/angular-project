import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlMstPincodeComponent } from './otl-mst-pincode.component';

describe('OtlMstPincodeComponent', () => {
  let component: OtlMstPincodeComponent;
  let fixture: ComponentFixture<OtlMstPincodeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlMstPincodeComponent]
    });
    fixture = TestBed.createComponent(OtlMstPincodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
