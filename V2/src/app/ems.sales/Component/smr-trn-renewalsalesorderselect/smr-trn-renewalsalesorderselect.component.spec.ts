import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnRenewalsalesorderselectComponent } from './smr-trn-renewalsalesorderselect.component';

describe('SmrTrnRenewalsalesorderselectComponent', () => {
  let component: SmrTrnRenewalsalesorderselectComponent;
  let fixture: ComponentFixture<SmrTrnRenewalsalesorderselectComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnRenewalsalesorderselectComponent]
    });
    fixture = TestBed.createComponent(SmrTrnRenewalsalesorderselectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
