import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysMstExpensecategorysummaryComponent } from './sys-mst-expensecategorysummary.component';

describe('SysMstExpensecategorysummaryComponent', () => {
  let component: SysMstExpensecategorysummaryComponent;
  let fixture: ComponentFixture<SysMstExpensecategorysummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SysMstExpensecategorysummaryComponent]
    });
    fixture = TestBed.createComponent(SysMstExpensecategorysummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
