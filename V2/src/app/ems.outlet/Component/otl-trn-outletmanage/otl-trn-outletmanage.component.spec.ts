import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlTrnOutletmanageComponent } from './otl-trn-outletmanage.component';

describe('OtlTrnOutletmanageComponent', () => {
  let component: OtlTrnOutletmanageComponent;
  let fixture: ComponentFixture<OtlTrnOutletmanageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlTrnOutletmanageComponent]
    });
    fixture = TestBed.createComponent(OtlTrnOutletmanageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
