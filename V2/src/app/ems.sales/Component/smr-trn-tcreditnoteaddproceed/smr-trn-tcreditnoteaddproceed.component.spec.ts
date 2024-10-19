import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnTcreditnoteaddproceedComponent } from './smr-trn-tcreditnoteaddproceed.component';

describe('SmrTrnTcreditnoteaddproceedComponent', () => {
  let component: SmrTrnTcreditnoteaddproceedComponent;
  let fixture: ComponentFixture<SmrTrnTcreditnoteaddproceedComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnTcreditnoteaddproceedComponent]
    });
    fixture = TestBed.createComponent(SmrTrnTcreditnoteaddproceedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
