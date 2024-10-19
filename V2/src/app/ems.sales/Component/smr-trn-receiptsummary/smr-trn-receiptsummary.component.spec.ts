import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnReceiptsummaryComponent } from './smr-trn-receiptsummary.component';

describe('SmrTrnReceiptsummaryComponent', () => {
  let component: SmrTrnReceiptsummaryComponent;
  let fixture: ComponentFixture<SmrTrnReceiptsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnReceiptsummaryComponent]
    });
    fixture = TestBed.createComponent(SmrTrnReceiptsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
