import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnJournalentryEditComponent } from './acc-trn-journalentry-edit.component';

describe('AccTrnJournalentryEditComponent', () => {
  let component: AccTrnJournalentryEditComponent;
  let fixture: ComponentFixture<AccTrnJournalentryEditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnJournalentryEditComponent]
    });
    fixture = TestBed.createComponent(AccTrnJournalentryEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
