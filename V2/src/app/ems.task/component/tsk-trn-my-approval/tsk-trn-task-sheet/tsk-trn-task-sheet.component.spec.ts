import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TskTrnTaskSheetComponent } from './tsk-trn-task-sheet.component';

describe('TskTrnTaskSheetComponent', () => {
  let component: TskTrnTaskSheetComponent;
  let fixture: ComponentFixture<TskTrnTaskSheetComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TskTrnTaskSheetComponent]
    });
    fixture = TestBed.createComponent(TskTrnTaskSheetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
