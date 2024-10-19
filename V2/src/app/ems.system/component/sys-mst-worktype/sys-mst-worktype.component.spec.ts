import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysMstWorktypeComponent } from './sys-mst-worktype.component';

describe('SysMstWorktypeComponent', () => {
  let component: SysMstWorktypeComponent;
  let fixture: ComponentFixture<SysMstWorktypeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SysMstWorktypeComponent]
    });
    fixture = TestBed.createComponent(SysMstWorktypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
