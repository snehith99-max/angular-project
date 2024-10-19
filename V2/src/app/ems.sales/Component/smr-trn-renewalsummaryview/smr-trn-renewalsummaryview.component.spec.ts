import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnRenewalsummaryviewComponent } from './smr-trn-renewalsummaryview.component';

describe('SmrTrnRenewalsummaryviewComponent', () => {
  let component: SmrTrnRenewalsummaryviewComponent;
  let fixture: ComponentFixture<SmrTrnRenewalsummaryviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnRenewalsummaryviewComponent]
    });
    fixture = TestBed.createComponent(SmrTrnRenewalsummaryviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
