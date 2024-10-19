import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnFundtransfersummaryComponent } from './acc-trn-fundtransfersummary.component';

describe('AccTrnFundtransfersummaryComponent', () => {
  let component: AccTrnFundtransfersummaryComponent;
  let fixture: ComponentFixture<AccTrnFundtransfersummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnFundtransfersummaryComponent]
    });
    fixture = TestBed.createComponent(AccTrnFundtransfersummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
