import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PblTrnDebitnoteSummaryComponent } from './pbl-trn-debitnote-summary.component';

describe('PblTrnDebitnoteSummaryComponent', () => {
  let component: PblTrnDebitnoteSummaryComponent;
  let fixture: ComponentFixture<PblTrnDebitnoteSummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PblTrnDebitnoteSummaryComponent]
    });
    fixture = TestBed.createComponent(PblTrnDebitnoteSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
