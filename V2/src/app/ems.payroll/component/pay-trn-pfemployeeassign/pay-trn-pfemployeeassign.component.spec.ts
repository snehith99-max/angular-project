import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayTrnPfemployeeassignComponent } from './pay-trn-pfemployeeassign.component';

describe('PayTrnPfemployeeassignComponent', () => {
  let component: PayTrnPfemployeeassignComponent;
  let fixture: ComponentFixture<PayTrnPfemployeeassignComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PayTrnPfemployeeassignComponent]
    });
    fixture = TestBed.createComponent(PayTrnPfemployeeassignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
