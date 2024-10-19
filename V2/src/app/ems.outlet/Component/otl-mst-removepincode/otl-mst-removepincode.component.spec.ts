import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlMstRemovepincodeComponent } from './otl-mst-removepincode.component';

describe('OtlMstRemovepincodeComponent', () => {
  let component: OtlMstRemovepincodeComponent;
  let fixture: ComponentFixture<OtlMstRemovepincodeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlMstRemovepincodeComponent]
    });
    fixture = TestBed.createComponent(OtlMstRemovepincodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
