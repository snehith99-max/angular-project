import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnIssueComponent } from './ims-trn-issue.component';

describe('ImsTrnIssueComponent', () => {
  let component: ImsTrnIssueComponent;
  let fixture: ComponentFixture<ImsTrnIssueComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnIssueComponent]
    });
    fixture = TestBed.createComponent(ImsTrnIssueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
