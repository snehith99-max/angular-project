import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnTcreditnnoteSummaryComponent } from './smr-trn-tcreditnnote-summary.component';

describe('SmrTrnTcreditnnoteSummaryComponent', () => {
  let component: SmrTrnTcreditnnoteSummaryComponent;
  let fixture: ComponentFixture<SmrTrnTcreditnnoteSummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnTcreditnnoteSummaryComponent]
    });
    fixture = TestBed.createComponent(SmrTrnTcreditnnoteSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
