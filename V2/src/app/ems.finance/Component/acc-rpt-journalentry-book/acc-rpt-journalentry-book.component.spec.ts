import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptJournalentryBookComponent } from './acc-rpt-journalentry-book.component';

describe('AccRptJournalentryBookComponent', () => {
  let component: AccRptJournalentryBookComponent;
  let fixture: ComponentFixture<AccRptJournalentryBookComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptJournalentryBookComponent]
    });
    fixture = TestBed.createComponent(AccRptJournalentryBookComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
